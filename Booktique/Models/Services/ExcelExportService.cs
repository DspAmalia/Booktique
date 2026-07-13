using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;

public class ExcelExportService
{
    public byte[] GenerateInvoicesExcel(List<InvoiceDto> invoices)
    {
        using (var workbook = new XLWorkbook())
        {
            // Creăm o foaie de calcul numită "Facturi"
            var worksheet = workbook.Worksheets.Add("Facturi");

            // 1. Adăugăm Header-ul tabelului
            worksheet.Cell(1, 1).Value = "Număr Factură";
            worksheet.Cell(1, 2).Value = "Dată Emitere";
            worksheet.Cell(1, 3).Value = "Dată Scadență";
            worksheet.Cell(1, 4).Value = "Client";
            worksheet.Cell(1, 5).Value = "Suma Netă (RON)";
            worksheet.Cell(1, 6).Value = "TVA (RON)";
            worksheet.Cell(1, 7).Value = "Total (RON)";
            worksheet.Cell(1, 8).Value = "Status";

            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#F1E6D8");

            // 2. Populăm datele din listă
            int currentRow = 2;
            foreach (var inv in invoices)
            {
                worksheet.Cell(currentRow, 1).Value = inv.InvoiceNumber;
                worksheet.Cell(currentRow, 2).Value = inv.IssueDate.ToString("dd.MM.yyyy");
                worksheet.Cell(currentRow, 3).Value = inv.DueDate.ToString("dd.MM.yyyy");
                worksheet.Cell(currentRow, 4).Value = inv.ClientName;
                worksheet.Cell(currentRow, 5).Value = inv.NetAmount;
                worksheet.Cell(currentRow, 6).Value = inv.VatAmount;
                worksheet.Cell(currentRow, 7).Value = inv.TotalAmount;
                worksheet.Cell(currentRow, 8).Value = inv.Status;

                // Formatăm coloanele de bani ca numere cu zecimale
                worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "#,##0.00";

                currentRow++;
            }

            // Ajustăm automat lățimea coloanelor în funcție de conținut
            worksheet.Columns().AdjustToContents();

            // 3. Salvăm workbook-ul într-un MemoryStream și returnăm vectorul de bytes
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}

// Un DTO (Data Transfer Object) 
public class InvoiceDto
{
    public string InvoiceNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public string ClientName { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
}