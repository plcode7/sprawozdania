
using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Schema;

// Run an XSLT
// You must include the following references in your VisualStudio project:
// System.dll
// System.Xml.dll
//

public class JednostkaInna {

	const String    inputUrl     = "file:///c:/Apps/Przykladowe-Sprawozdanie.xml";
	const String    outputFile   = "c:\\Apps\\index.html";
	const String    xsltUri      = "file:///c:/Apps/JednostkaInna.xsl";

	public static void Main(String[] args) {

		try {

			XslCompiledTransform transformer = new XslCompiledTransform();
			transformer.Load(xsltUri, new XsltSettings(true, true),null);

			XsltArgumentList xsltArguments = new XsltArgumentList();
			xsltArguments.AddParam("procesor", "", "");
			xsltArguments.AddParam("schema-pkd", "", "");
			xsltArguments.AddParam("root", "", "");
			xsltArguments.AddParam("schema-podatek", "", "");


			XmlWriterSettings settings = transformer.OutputSettings.Clone();
			settings.CloseOutput = true;
			using(XmlWriter outputWriter = XmlWriter.Create(outputFile, settings)) {

				Console.WriteLine();
				Console.WriteLine("XSLT starting.");
				transformer.Transform(inputUrl,
				                      xsltArguments,
				                      outputWriter);
				Console.WriteLine();
				Console.WriteLine("XSLT finished.");

			}

			Console.WriteLine();
			Console.WriteLine("Validation starting.");
			XmlReaderSettings rdrSettings = new XmlReaderSettings();
			rdrSettings.ValidationType = ValidationType.Schema;
			rdrSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
			rdrSettings.ValidationEventHandler += new ValidationEventHandler (ValidationCallBack);
			try {
				using (XmlReader validator = XmlReader.Create(outputFile, rdrSettings)) {
					while(validator.Read())
					;
				}
			} catch(XmlSchemaValidationException e) {
				Console.WriteLine("The schema is not valid.");
				Console.WriteLine(e.Message);
			}
			Console.WriteLine();
			Console.WriteLine("Validation finished.");

		} catch(Exception e) {
			Console.Error.WriteLine(e.ToString());
			// Console.Error.WriteLine(e.StackTrace);
		}
	}

	// Display any validation errors.
	private static void ValidationCallBack(object sender, ValidationEventArgs e) {
		Console.WriteLine("Validation Error: {0}", e.Message);
	}
}
