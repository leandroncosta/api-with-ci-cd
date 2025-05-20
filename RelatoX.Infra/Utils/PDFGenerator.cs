using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RelatoX.Application.DTOs;
using RelatoX.Domain.Enums;

namespace RelatoX.Infra.Utils
{
    public class PDFGenerator
    {
        public static byte[]? Generate<T>(IEnumerable<T> reports)
        {
            var document = Document.Create(container =>
            {
                Console.WriteLine($"Quantidade de relatórios: {reports.Count()}");

                container.Page(page =>
                {
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(60); // Month
                            columns.RelativeColumn();   // Total
                            columns.RelativeColumn();   // Count
                            columns.RelativeColumn();   // Type
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Ano");
                            header.Cell().Element(CellStyle).Text("Month");
                            header.Cell().Element(CellStyle).Text("Type");
                            header.Cell().Element(CellStyle).Text("Total consumo");
                            header.Cell().Element(CellStyle).Text("Unit");
                            header.Cell().Element(CellStyle).Text("Entries");
                            header.Cell().Element(CellStyle).Text("Valor total R$");
                        });

                        foreach (var report in reports)
                        {
                            dynamic r = report!;
                            table.Cell().Element(CellStyle).Text(new DateTime((int)r.Year, 1, 1).ToString("YYY"));
                            table.Cell().Element(CellStyle).Text(new DateTime(1, (int)r.Month, 1).ToString("MMM"));
                            table.Cell().Element(CellStyle).Text((string)r.TotalConsumption.ToString("F2"));
                            table.Cell().Element(CellStyle).Text((string)r.EntryCount.ToString());
                            table.Cell().Element(CellStyle).Text((ConsumptionType)r.Type);
                        }
                    });
                });
            });

            return document.GeneratePdf();
            //File.WriteAllBytes("relatorio_teste.pdf", document.GeneratePdf());
        }

        public static byte[]? Generate(List<ReportDto> reports)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Header().Text("Relatório de Consumo");
                    //.FontSize(22)
                    //    .Bold()
                    //    .FontColor(Colors.Blue.Medium)
                    //    .AlignCenter()
                     //   .PaddingBottom(10);

                    page.Content().Column(column =>
                    {
                        var ano = reports.FirstOrDefault()?.Year ?? 0;
                        var custoTotal = reports.Sum(r => r.TotalCost);

                        // Linha de resumo
                        column.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Background(Colors.Grey.Lighten4).Row(row =>
                        {
                            row.RelativeColumn().Text($"Ano: {ano}")
                                .FontSize(14).SemiBold().FontColor(Colors.Grey.Darken3);
                            row.RelativeColumn().AlignRight().Text($"Custo Total: R$ {custoTotal:F2}")
                                .FontSize(14).SemiBold().FontColor(Colors.Grey.Darken3);
                        });
                        //.PaddingBottom(15);

                        // Tabela
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); // Ano
                                columns.RelativeColumn(); // Mês
                                columns.RelativeColumn(); // Tipo
                                columns.RelativeColumn(); // Consumo
                                columns.RelativeColumn(); // Unidade
                                columns.RelativeColumn(); // Entradas
                                columns.RelativeColumn(); // Valor total
                            });

                            // Cabeçalho
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyleHeader).Text("Ano");
                                header.Cell().Element(CellStyleHeader).Text("Mês");
                                header.Cell().Element(CellStyleHeader).Text("Tipo");
                                header.Cell().Element(CellStyleHeader).Text("Consumo Total");
                                header.Cell().Element(CellStyleHeader).Text("Unidade");
                                header.Cell().Element(CellStyleHeader).Text("Entradas");
                                header.Cell().Element(CellStyleHeader).Text("Valor Total (R$)");
                            });

                            // Linhas da tabela
                            foreach (var r in reports)
                            {
                                table.Cell().Element(CellStyle).Text(r.Year.ToString());
                                table.Cell().Element(CellStyle).Text(new DateTime(1, (int)r.Month, 1).ToString("MMM"));
                                table.Cell().Element(CellStyle).Text(((ConsumptionType)r.Type).ToString());
                                table.Cell().Element(CellStyle).Text(r.TotalConsumption.ToString("F2"));
                                table.Cell().Element(CellStyle).Text(r.Unit);
                                table.Cell().Element(CellStyle).Text(r.EntryCount.ToString());
                                table.Cell().Element(CellStyle).Text(r.TotalCost.ToString("F2"));
                            }
                        });
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Relatório gerado em ").FontColor(Colors.Grey.Darken1);
                            text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).SemiBold();
                        });
                        //.FontSize(10).PaddingTop(20);
                });
            });

            return document.GeneratePdf();
        }

        // Estilos de célula
        static IContainer CellStyle(IContainer container) =>
            container
                .PaddingVertical(5)
                .PaddingHorizontal(3)
                .AlignMiddle()
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2);

        static IContainer CellStyleHeader(IContainer container) =>
            container
                .Background(Colors.Grey.Lighten3)
                .PaddingVertical(5)
                .PaddingHorizontal(3)
                .AlignMiddle()
                .BorderBottom(1.5f)
                .BorderColor(Colors.Grey.Darken1)
            .AlignCenter()
                //.TextAlignment(TextAlignment.Center)
                .DefaultTextStyle(x => x.SemiBold().FontColor(Colors.Grey.Darken4));


        public static byte[]? GenerateByUser(MonthlyUserReportDto report)
        {
            var document = Document.Create(container =>
            {
                Console.WriteLine($"Quantidade de relatórios: {report.Summary.Count()}");

                container.Page(page =>
                {
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(60); // Month
                            columns.RelativeColumn();   // Water
                            columns.RelativeColumn();   // Gas
                            columns.RelativeColumn();   // Energy
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Month");
                            header.Cell().Element(CellStyle).Text("Water");
                            header.Cell().Element(CellStyle).Text("Gas");
                            header.Cell().Element(CellStyle).Text("Energy");
                        });

                        foreach (var report in report.Summary)
                        {
                            dynamic r = report!;
                            table.Cell().Element(CellStyle).Text(new DateTime(1, int.Parse(report.Month), 1).ToString("MMM"));
                            table.Cell().Element(CellStyle).Text((string)r.Water.ToString("F2"));
                            table.Cell().Element(CellStyle).Text((string)r.Gas.ToString("F2"));
                            table.Cell().Element(CellStyle).Text((string)r.Energy.ToString("F2"));
                        }
                    });
                });
            });

            return document.GeneratePdf();
            //File.WriteAllBytes("relatorio_teste.pdf", document.GeneratePdf());
        }

        //private static IContainer CellStyle(IContainer container)
        //{
        //    return container
        //        .Padding(1)
        //        // .Border(1)
        //        //.BorderColor(Colors.Grey.Lighten2)
        //        .AlignCenter()
        //        .AlignMiddle();
        //}
    }
}

//public class MonthlyUserReportDto
//{
//    public string UserId { get; set; } = default!;
//    public int Year { get; set; }
//    public List<MonthlyConsumptionSummaryDto> Summary { get; set; } = new();
//}

//public class MonthlyConsumptionSummaryDto
//{
//    public string Month { get; set; } = default!;
//    public double Water { get; set; }
//    public double Gas { get; set; }
//    public double Energy { get; set; }
//}