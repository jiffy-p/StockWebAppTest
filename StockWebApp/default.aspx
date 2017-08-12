<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="StockWebApp._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Web App</title>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="content">
        <span>DEFAULT PAGE</span>
        <div class="tableChartContainer">
            <div id="TableChart" class="tableChart"></div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        google.charts.load("current", { packages: ["bar", "corechart", "gauge", "table", "controls"] });
        google.charts.setOnLoadCallback(draw_TableChart);

        function draw_TableChart() {
            var chartId = "TableChart";

            // Create the data table.
            var jsonData = <%= this.Json_TableChart %>;

            if (jsonData) {
                var data = new google.visualization.DataTable(jsonData);

                // Options
                var options = {
                    allowHtml: true,
                    width: "100%",
                    height: "100%",
                    sortColumn: 0,
                    sortAscending: false
                };

                var chart = new google.visualization.Table(document.getElementById(chartId));
                chart.draw(data, options);
            }
        }

    </script>
    </form>
</body>
</html>
