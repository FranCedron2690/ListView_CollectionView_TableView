using System;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace ListView_CollectionView_TableView.Helpers
{
    public class PDF_Tools
    {
        string pathLocalResourcesAndroid = "";

        public PDF_Tools()
        {
            //CreateNewPDF(filePath1);

            //if (File.Exists(filePath1))
            //{
            //    if (File.Exists(filePath2))
            //    {
            //        MixPDFs(filePath1, filePath2);
            //    }
            //    else
            //        Debug.WriteLine("El fichero: " + filePath2 + "no existe en la ruta actual");
            //}
            //else
            //    Debug.WriteLine("El fichero: " + filePath1 + "no existe en la ruta actual");
        }

        public static PDF_Tools Instance { get; } = new PDF_Tools();

        private void MixPDFs(string pathPDF1, string pathPDF2)
        {
            string NewMixedPDFfilePath = Path.Combine(pathLocalResourcesAndroid, "NewPDF.pdf");

            PdfReader pdfReader1 = new PdfReader(pathPDF1);
            PdfReader pdfReader2 = new PdfReader(pathPDF2);

            //Memorystream usado para writer del nuevo pdf. Todo el contenido añadido al pdf será
            //almacenado en el memoryStream al ser asociado en la instancia del PDFWriter
            MemoryStream memoryStream = new MemoryStream();
            Document newPDF = new Document(PageSize.LETTER);

            var writer = new PdfCopy(newPDF, memoryStream);
            writer.AddViewerPreference(PdfName.PICKTRAYBYPDFSIZE, PdfBoolean.PDFTRUE);// Añadimos los atributos del nuevo PDF

            //Nuevo PDF
            newPDF.AddAuthor("Fran");
            newPDF.AddTitle("Mezcla de fichero PDF");
            newPDF.AddCreator("Fran Cedron");
            newPDF.AddCreationDate();

            // Abrimos el documento
            newPDF.Open();

            //PDF1
            CopyPDFPages(pdfReader1.NumberOfPages, writer, pdfReader1);

            //PDF2
            CopyPDFPages(pdfReader1.NumberOfPages, writer, pdfReader2);

            newPDF.Close();
            writer.Close();// En el caso que querramos guardarlo en una carpeta

            byte[] bytArr = memoryStream.ToArray();//array de bytes con el contenido del pdf almacenado en el memory stream

            using (FileStream fs = File.Create(NewMixedPDFfilePath))
            {
                fs.Write(bytArr, 0, (int)bytArr.Length);
            }

            Debug.WriteLine("El fichero existe: creando mezcla fichero");
        }

        private void CopyPDFPages(int numberPagestoCopy, PdfCopy writer, PdfReader reader)
        {
            PdfImportedPage page;

            for (int i = 1; i <= numberPagestoCopy; i++)
            {
                page = writer.GetImportedPage(reader, i);//página a importar del documento
                writer.AddPage(page);
            }
        }

        public static void CreateNewPDF(string filePath)
        {
            //Byte[] img_firma = Convert.FromBase64String(dataPDF.SignatureBase64);

            //Datos variables pdf
            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Now.Month).ToUpper();
            //monthName = monthName.Substring(0, 0).ToUpper() + monthName.Substring(0, monthName.Length);
            string dateMonthDayTable = (monthName.Substring(0, 3)) + "/ " + DateTime.Now.Day;

            ///Crear un fichero pdf en la carpeta local del usuario en Android
            Document doc = new Document(PageSize.A4, 50, 40, 30, 30);//Tamaño página, borders izq, dcha, sup, inf
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            int indentLeft = 27;

            //Meta información pdf
            doc.AddAuthor("iAR");
            doc.AddCreator("iAR Software");
            doc.AddKeywords("PDF plantilla firma");
            doc.AddSubject("Plantilla PDF generado para firmar y enviar al servidor");
            doc.AddTitle("PDF Firma");

            //Se abre el documento
            doc.Open();

            ////Leer imagen embebida Tabla
            //Image imgTablePDF = Instance.LoadEmbeddedImage("EmbeddedImages.TablaInicialDocumentoPDF.png");
            //imgTablePDF.ScalePercent(75);//Escala imagen
            //imgTablePDF.Alignment = Element.ALIGN_CENTER;
            //doc.Add(imgTablePDF);

            //Tabla cabezera
            PdfPTable tableHeader = new PdfPTable(3);//tabla con 3 columnas
            tableHeader.HorizontalAlignment = 1;
            tableHeader.TotalWidth = 510;
            tableHeader.LockedWidth = true;
            float[] widthsTableHeader = { 80f, 330f, 100f };
            tableHeader.SetWidths(widthsTableHeader);

            //1 columna logp
            //Image logo = Instance.LoadEmbeddedImage("EmbeddedImages.Icon_inerziaPDF.png");
            //logo.ScalePercent(85);

            PdfPCell cell;
            //PdfPCell cell = new PdfPCell(logo)
            //{
            //    HorizontalAlignment = Element.ALIGN_CENTER,
            //    VerticalAlignment = Element.ALIGN_MIDDLE
            //};
            //cell.Border = Rectangle.BOX;
            //cell.Rowspan = 3;
            //tableHeader.AddCell(cell);

            //2 columna Texto: FORMACIÓN/INFORMACIÓN A TRABAJADORES
            cell = Instance.CreateCell("FORMACIÓN/INFORMACIÓN A TRABAJADORES", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX, Color.WHITE);
            cell.Rowspan = 3;
            tableHeader.AddCell(cell);

            //3 Columna
            tableHeader.AddCell(Instance.CreateCell("F-GI-PRL-32", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX, Color.WHITE));
            tableHeader.AddCell(Instance.CreateCell(dateMonthDayTable, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX, Color.WHITE));
            tableHeader.AddCell(Instance.CreateCell("Rev: 02", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX, Color.WHITE));


            doc.Add(tableHeader);

            //Salto de linea
            Instance.BreakLines(doc, 1);

            //Formación/Información
            PdfPTable form_infoTable = new PdfPTable(1);//tabla con 3 columnas
            form_infoTable.HorizontalAlignment = 1;
            form_infoTable.TotalWidth = 510;
            Phrase form_inforPhrase = new Phrase();
            Font whiteText = new Font(Font.TIMES_ROMAN, 12, Font.NORMAL, Color.WHITE);
            Font grayText = new Font(Font.TIMES_ROMAN, 12, Font.NORMAL, Color.LIGHT_GRAY);
            Chunk chunk_whiteText = new Chunk("FORMACIÓN / ", whiteText);
            Chunk chunk_grayText = new Chunk("INFORMACIÓN", grayText);
            form_inforPhrase.Add(chunk_whiteText);
            form_inforPhrase.Add(chunk_grayText);

            cell = new PdfPCell(form_inforPhrase)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BackgroundColor = Color.BLACK
            };

            form_infoTable.AddCell(cell);

            //form_infor.Alignment = Element.ALIGN_CENTER;
            doc.Add(form_infoTable);

            //Saltos de linea
            Instance.BreakLines(doc, 2);

            //Primer parrafo
            Paragraph first_paragraph = new Paragraph("En cumplimiento de la normativa legal en materia " +
                "de Seguridad y Salud Laboral, y de acuerdo con la política interna de Grupo Inerzia en " +
                "materia de comunicación");
            first_paragraph.FirstLineIndent = indentLeft;
            doc.Add(first_paragraph);

            //Saltos de linea
            Instance.BreakLines(doc, 1);

            //Nombre representante empresa y DNI
            Paragraph data_factory = new Paragraph();
            data_factory.Add("Cedron S.A");
            data_factory.Add(" con DNI: ");
            data_factory.Add("44645940L");
            data_factory.IndentationLeft = 17;
            data_factory.IndentationRight = 17;
            doc.Add(data_factory);

            //Saltos de linea
            Instance.BreakLines(doc, 1);

            //Segundo parrafo
            Paragraph second_paragraph = new Paragraph();// ("En nombre y representación de la empresa ______”empresa”" +
                                                         //"___________ perteneciente a Grupo Inerzia");
            second_paragraph.Add("En nombre y representación de la empresa ");
            second_paragraph.Add("Cedron S.A");
            second_paragraph.Add("perteneciente a Grupo Inerzia");
            second_paragraph.FirstLineIndent = indentLeft;
            doc.Add(second_paragraph);

            //Saltos de linea
            Instance.BreakLines(doc, 1);

            //Tercer parrafo
            Paragraph third_paragraph = new Paragraph();// ("Hace entrega a _____”Nombre del trabajador”_____________ " +
                                                        //"con DNI: __”DNI trabajador”___ y trabajador de la empresa ______”empresa”__________ del " +
                                                        //"siguiente documento;");
            third_paragraph.Add("Hace entrega a ");
            third_paragraph.Add("Francisco Cedrón");
            third_paragraph.Add(" con DNI: ");
            third_paragraph.Add("44645940L");
            third_paragraph.Add(" y trabajador de la empresa ");
            third_paragraph.Add("Cedron S.A");
            third_paragraph.Add(" del siguiente documento:");
            third_paragraph.FirstLineIndent = indentLeft;
            doc.Add(third_paragraph);

            //Saltos de linea
            Instance.BreakLines(doc, 1);

            //Cuarto parrafp
            PdfPTable nameDocumentTable = new PdfPTable(1);
            cell = new PdfPCell(new Phrase("Documento de prueba"))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = Rectangle.BOTTOM_BORDER
            };
            //Paragraph fourth_paragraph = new Paragraph("_____________”Nombre del documento entregado”__________________________");
            //fourth_paragraph.IndentationLeft = 10;
            //fourth_paragraph.IndentationRight = 10;
            //doc.Add(fourth_paragraph);

            //Imagen Firma
            // Creamos la imagen y le ajustamos el tamaño
            //Image imagen = Image.GetInstance(img_firma);
            //Image imagen = Image.GetInstance("localización de la imagen");
            //imagen.BorderWidth = 0;
            //imagen.Alignment = Element.ALIGN_RIGHT;

            //Saltos de linea
            Instance.BreakLines(doc, 1);

            //Quinto parrafo
            Paragraph fifth_paragraph = new Paragraph();// ("Igualmente, el trabajador _______”Nombre del trabajador”______ " +
                                                        //"con DNI: ___”DNI trabajador___ se compromete a leer y cumplir fielmente todas y cada una de las normas " +
                                                        //"y obligaciones recogidas en dicho documento");
            fifth_paragraph.Add("Igualmente, el trabajador ");
            fifth_paragraph.Add("Francisco Cedron");
            fifth_paragraph.Add(" con DNI ");
            fifth_paragraph.Add("44645940L");
            fifth_paragraph.Add(" se compromete a leer y cumplir fielmente todas y cada una de las normas y obligaciones " +
                "recogidas en dicho documento");
            fifth_paragraph.FirstLineIndent = indentLeft;
            doc.Add(fifth_paragraph);

            //Saltos de linea
            Instance.BreakLines(doc, 6);

            //Texto Firma
            Paragraph phrase_info_sign = new Paragraph("Para que conste se firma el presente documento");
            doc.Add(phrase_info_sign);
            phrase_info_sign.IndentationLeft = indentLeft;
            Instance.BreakLines(doc, 1);

            Paragraph sign_worker_text = new Paragraph("Firma del trabajador");
            sign_worker_text.IndentationLeft = 50;
            doc.Add(sign_worker_text);

            //Saltos de linea
            Instance.BreakLines(doc, 2);

            //Insertamos la imagen en el documento
            //imagen.Alignment = Element.ALIGN_CENTER;
            //imagen.ScalePercent(30);
            //doc.Add(imagen);

            //Saltos de linea
            Instance.BreakLines(doc, 2);

            //Lugar y fecha de la firma
            PdfPTable tablePlaceDate = new PdfPTable(8);

            tablePlaceDate.HorizontalAlignment = 0;
            tablePlaceDate.TotalWidth = 500f;
            tablePlaceDate.LockedWidth = true;
            float[] widthsTablePlaceDate = { 13f, 60f, 15f, 30f, 15f, 60f, 15f, 50f };
            tablePlaceDate.SetWidths(widthsTablePlaceDate);

            tablePlaceDate.DefaultCell.Border = Rectangle.NO_BORDER;

            tablePlaceDate.AddCell(Instance.CreateCell("En", Element.ALIGN_CENTER, Element.ALIGN_UNDEFINED, 0, Color.WHITE));

            string location = "Pamplona";
            tablePlaceDate.AddCell(Instance.CreateCell(location, Element.ALIGN_CENTER, Element.ALIGN_UNDEFINED, 0, Color.LIGHT_GRAY));

            tablePlaceDate.AddCell(Instance.CreateCell(" a ", Element.ALIGN_CENTER, Element.ALIGN_UNDEFINED, 0, Color.WHITE));

            string dayNumber = DateTime.Now.Day.ToString();
            tablePlaceDate.AddCell(Instance.CreateCell(dayNumber, Element.ALIGN_CENTER, Element.ALIGN_UNDEFINED, 0, Color.LIGHT_GRAY));

            tablePlaceDate.AddCell(Instance.CreateCell(" de ", Element.ALIGN_CENTER, Element.ALIGN_UNDEFINED, 0, Color.WHITE));

            string nameMonth = DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Now.Month);
            tablePlaceDate.AddCell(Instance.CreateCell(nameMonth, Element.ALIGN_CENTER, Element.ALIGN_UNDEFINED, 0, Color.LIGHT_GRAY));

            tablePlaceDate.AddCell(Instance.CreateCell(" de ", Element.ALIGN_CENTER, Element.ALIGN_UNDEFINED, 0, Color.WHITE));

            string year = DateTime.Now.Year.ToString();
            tablePlaceDate.AddCell(Instance.CreateCell(year, Element.ALIGN_CENTER, Element.ALIGN_UNDEFINED, 0, Color.LIGHT_GRAY));

            //Se añade la tabla
            doc.Add(tablePlaceDate);

            doc.Close();
        }

        private void BreakLines(Document doc, int num_lines)
        {
            Paragraph emptyLine = new Paragraph(" ");

            for (int i = 0; i < num_lines; i++)
                doc.Add(emptyLine);
        }

        private PdfPCell CreateCell(string content, int horizontalAlign, int verticallAlign, int border, Color color)
        {
            PdfPCell cell = new PdfPCell(new Phrase(content));
            cell.HorizontalAlignment = horizontalAlign;
            cell.VerticalAlignment = verticallAlign;
            cell.Border = border;
            cell.BackgroundColor = color;

            return cell;
        }

        private Image LoadEmbeddedImage(string nameImg)
        {
            string resourcePrefix = "INERZIA.";

            string pathImagexTable = resourcePrefix + nameImg;//"EmbeddedImages.imagen.jpg";

            Assembly assembly = GetType().GetTypeInfo().Assembly;

            Image imagen_table;

            using (Stream stream = assembly.GetManifestResourceStream(pathImagexTable))
            {
                imagen_table = Image.GetInstance(stream);
            }

            return imagen_table;
        }
    }
}