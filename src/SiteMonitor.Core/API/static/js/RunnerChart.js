var RunnerChart = {
    initialize: function (chartObj, chartTitle, YAxisLegend) {
        this.options = {
            title: chartTitle,
            titleTextStyle: { color: 'white' },
            backgroundColor: { fill: 'black' },
            colors: ['orange'],
            areaOpacity: .6,
            chartArea: { 'left': 40, 'top': 100, 'width': '80%' },
            width: $(window).width(),
            height: $(window).height(),
            animation: { duration: 500, easing: 'out' },
            vAxis: { minValue: 0, textStyle: { color: 'white' } },
            hAxis: { textStyle: { color: 'white' } },
            legend: { textStyle: { color: 'white' } }
        }

        this.chart = new google.visualization.AreaChart(chartObj);
        this.dataTable = new google.visualization.DataTable();
        this.dataTable.addColumn('string', 'string');
        this.dataTable.addColumn('number', YAxisLegend);
    },

    render: function (data) {
        var that = this;

        $.each(data, function (index, obj) {
            if (that.dataTable)
                that.dataTable.addRow([obj[0], obj[1]])
        });

        this.chart.draw(this.dataTable, this.options)
    }
}