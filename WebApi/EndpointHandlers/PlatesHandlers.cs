using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Security.Claims;
using WebApi.DbContexts;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.EndpointHandlers
{
    public class PlatesHandlers
    {
        public static async Task<Ok<IEnumerable<PlateDto>>> GetPlatesAsync(
            PlatesDbContext platesDbContext,
            IMapper mapper,
            string? licenseText)
        {
            return TypedResults.Ok(mapper.Map<IEnumerable<PlateDto>>(await platesDbContext.Plates
                .Where(p => licenseText == null || p.LicenseText.Contains(licenseText))
                .ToListAsync()));
        }

        public static async Task<Results<NotFound, Ok<PlateDto>>> GetPlateByIdAsync(
            PlatesDbContext platesDbContext,
            Guid plateId,
            IMapper mapper)
        {
            var plateEntity = await platesDbContext.Plates.FirstOrDefaultAsync(p => p.Id == plateId);
            if (plateEntity == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(mapper.Map<PlateDto>(plateEntity));
        }

        public static async Task<Ok<PlateDto>> GetPlateByLicenseTextAsync(
            PlatesDbContext platesDbContext,
            string licenseText,
            IMapper mapper)
        {
            return TypedResults.Ok(mapper.Map<PlateDto>(await platesDbContext.Plates.FirstOrDefaultAsync(p => p.LicenseText == licenseText)));
        }

        public static async Task<CreatedAtRoute<PlateDto>> CreatePlateAsync(
           PlatesDbContext platesDbContext,
           IMapper mapper,
           PlateForCreationDto plateForCreationDto)
        {
            var plateEntity = mapper.Map<Plate>(plateForCreationDto);

            //string formattedStartTime = $"{DateTime.UtcNow.ToString("dd.MM.yyyy")}";
            
            plateEntity.StartTime = DateTime.UtcNow;
            plateEntity.IsActive = true;

            platesDbContext.Add(plateEntity);
            await platesDbContext.SaveChangesAsync();

            var plateToReturn = mapper.Map<PlateDto>(plateEntity);

            return TypedResults.CreatedAtRoute(plateToReturn, "GetPlate", new
            {
                plateId = plateToReturn.Id
            });
        }

        public static async Task<Results<NotFound, NoContent>> UpdatePlateAsync(
            PlatesDbContext platesDbContext,
            IMapper mapper,
            Guid plateId,
            PlateForUpdateDto plateForUpdateDto)
        {
            var plateEntity = await platesDbContext.Plates.FirstOrDefaultAsync(p => p.Id == plateId);
            if (plateEntity == null)
            {
                return TypedResults.NotFound();
            }

            if (plateForUpdateDto.LicenseText == null)
            {
                plateForUpdateDto.LicenseText = plateEntity.LicenseText;
            }

            mapper.Map(plateForUpdateDto, plateEntity);

            await platesDbContext.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        public static async Task<Results<NotFound, BadRequest, NoContent>> UpdatePlateToNotActiveAsync(
             PlatesDbContext platesDbContext,
             IMapper mapper,
             Guid plateId,
             PlateForEndWorkDto plateForEndWorkDto)
        {
            var plateEntity = await platesDbContext.Plates.FirstOrDefaultAsync(p => p.Id == plateId);
            if (plateEntity == null)
            {
                return TypedResults.NotFound();
            }

            if (plateForEndWorkDto.IsActive)
            {
                return TypedResults.BadRequest();
            }

            plateForEndWorkDto.EndTime = DateTime.UtcNow;
            plateForEndWorkDto.WorkedTime = plateForEndWorkDto.EndTime - plateEntity.StartTime;

            mapper.Map(plateForEndWorkDto, plateEntity);

            await platesDbContext.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        public static async Task<Results<NotFound, NoContent>> DeletePlateAsync(
            PlatesDbContext platesDbContext,
            Guid plateId)
        {
            var plateEntity = await platesDbContext.Plates.FirstOrDefaultAsync(p => p.Id == plateId);
            if (plateEntity == null)
            {
                return TypedResults.NotFound();
            }
            platesDbContext.Plates.Remove(plateEntity);
            await platesDbContext.SaveChangesAsync();

            return TypedResults.NoContent();
        }
        
        public static async Task<IResult> GetExportedPlatesFullAsync(
            PlatesDbContext platesDbContext)
        {
            var plates = await platesDbContext.Plates.ToListAsync();
            
            var platesToExport = plates.OrderBy(plate => plate.StartTime.Year)
                                      .ThenBy(plate => plate.StartTime.Month)
                                      .ThenBy(plate => plate.StartTime.Day)
                                      .GroupBy(plate => new { plate.StartTime.Year, plate.StartTime.Month});                                    

            //  Create new Excel file
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            using (var package = new ExcelPackage())
            {
                foreach(var group in platesToExport)
                {
                    string worksheetName = $"{new DateTime(group.Key.Year, group.Key.Month, 1).ToString("MM-yyyy")}";
                    
                    var worksheet = package.Workbook.Worksheets.Add(worksheetName);

                    //  Format table header
                    worksheet.Cells[1, 1].Value = "SPZ";
                    worksheet.Cells[1, 2].Value = "Datum příjezdu";
                    worksheet.Cells[1, 3].Value = "Čas příjezdu";
                    worksheet.Cells[1, 4].Value = "Datum odjezdu";
                    worksheet.Cells[1, 5].Value = "Čas odjezdu";
                    worksheet.Cells[1, 6].Value = "Odpracovaný čas";
                    worksheet.Cells[1, 7].Value = "Nakládka / Vykládka";

                    using (var range = worksheet.Cells[1, 1, 1, 7])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    //  Fill Excel table with data
                    int row = 2;
                    foreach (var plate in group)
                    {
                        worksheet.Cells[row, 1].Value = plate.LicenseText;
                        worksheet.Cells[row, 2].Value = plate.StartTime.ToString("dd.MM.yyyy");
                        worksheet.Cells[row, 3].Value = plate.StartTime.ToString("HH:mm:ss");
                        worksheet.Cells[row, 4].Value = plate.EndTime.ToString("dd.MM.yyyy");
                        worksheet.Cells[row, 5].Value = plate.EndTime.ToString("HH:mm:ss");
                        worksheet.Cells[row, 6].Value = $"{(int)plate.WorkedTime.TotalHours}:{plate.WorkedTime.Minutes:D2}:{plate.WorkedTime.Seconds:D2}";
                        worksheet.Cells[row, 7].Value = plate.IsLoading ? "Nakládka" : "Vykládka";
                        row++;
                    }

                    //  Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                //  Returning Excel file as result
                package.SaveAs(stream);
                stream.Position = 0;
            }

            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Registr SPZ.xlsx";

            return Results.File(stream, contentType, fileName);
        }
    }
}
