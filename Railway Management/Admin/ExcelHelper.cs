using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using static Railway_Management.Models.AllDataDetails;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace Railway_Management.Admin
{
    public class ExcelHelper
    {
        public List<TrainSchedule> ReadExcel(string filePath)
        {
            var trainSchedules = new List<TrainSchedule>();

            try
            {
                // LicenseContext set karna for non-commercial use
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // FileInfo object create karte hain
                FileInfo fileInfo;
                try
                {
                     fileInfo = new FileInfo(filePath);

                    Console.WriteLine("datais reading now");

                    using(var package= new ExcelPackage(fileInfo))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rows = worksheet.Dimension.End.Row;
                        var columns = worksheet.Dimension.End.Column;
                        // Loop through rows starting from the second row
                        for (int row = 2; row <= rows; row++) // Assuming first row is header
                        {
                            Console.WriteLine(worksheet.Cells[row, 2].Text?.ToString());
                            Console.WriteLine(worksheet.Cells[row, 2].Text?.ToString());
                            var trainSchedule = new TrainSchedule
                            {
                                Arrival = worksheet.Cells[row, 1].Text?.ToString(),

                                Day = int.TryParse(worksheet.Cells[row, 2].Text?.ToString(), out int day) ? day : 0, // Default to 0 if parsing fails

                                Train_Name = worksheet.Cells[row, 3].Text?.ToString() ?? "Unknown", // Default to "Unknown" if null

                                Station_Name = worksheet.Cells[row, 4].Text?.ToString() ?? "Unknown", // Default to "Unknown" if null

                                Station_Code = worksheet.Cells[row, 5].Text?.ToString() ?? "N/A", // Default to "N/A" if null

                                Id = int.TryParse(worksheet.Cells[row, 6].Text?.ToString(), out int id) ? id : 0, // Default to 0 if parsing fails

                                Train_Number = worksheet.Cells[row, 7].Text?.ToString(),

                                Departure = TimeSpan.TryParse(worksheet.Cells[row, 8].Text?.ToString(), out TimeSpan departure) ? departure : TimeSpan.Zero // Default to TimeSpan.Zero if parsing fails
                            };


                            trainSchedules.Add(trainSchedule);
                        }
                        Console.WriteLine("hi ");
                    }
                     Console.WriteLine("");

                    return trainSchedules;
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    throw new Exception("Error processing Excel file: " + ex.Message);
                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error processing Excel file: " + ex.Message);

            }

            //return trainSchedules;
        }
    }

    // TrainSchedule model for representing each row data
   
}
