using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQL_Designer.Gui;

namespace SQL_DESIGNER.SaveSystem;
public static class SqlExporter {
  public static void ExportToSql(App app) {
    var tables = new StringBuilder();
    foreach (var p in app.Panels) {
      tables.AppendLine(CreateTable((Panel)p));
    }

    var data = tables.ToString();

    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    var path = Path.Combine(documents, "pmarkowski/sqlDesigner/exports");
    Directory.CreateDirectory(path);

    var filename = $"export.sql";
    File.WriteAllText(Path.Combine(path, filename), data);
  }

  private static string CreateTable(Panel panel) {
    var tableData = new StringBuilder();
    tableData.AppendLine($"CREATE TABLE `{panel.GetTitle()}` (");

    var inputFields = panel.GetInputFields();

    for (short i = 0; i < inputFields.Count; i++) {
      var additionalInfo = "";
      var dot = ",";

      if (i == inputFields.Count - 1) {
        dot = "";
      }

      var infoClass = inputFields[i].FieldInfo;
      if (infoClass.NotNull) {
        additionalInfo += "NOT NULL ";
      }
      if (infoClass.AutoIncrement) {
        additionalInfo += "AUTO_INCREMENT ";
      }
      if (infoClass.PrimaryKey) {
        additionalInfo += "PRIMARY KEY ";
      }

      var targetInputType = inputFields[i].FieldType.ToString().ToUpper();
      additionalInfo = additionalInfo.TrimEnd();

      tableData.AppendLine($"{inputFields[i].GetContent()} {targetInputType} {additionalInfo}{dot}");
    }

    tableData.AppendLine(");");
    return tableData.ToString();
  }
}
