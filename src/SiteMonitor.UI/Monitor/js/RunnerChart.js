var RunnerChart = {
    initialize: function (chartObj) {
        this.options = {
            title: '',
            colors: ['green'],
            areaOpacity: .6,
            chartArea: { 'top': 10, 'width': '80%' },
            width: "100%",
            height: 300,
            animation: { duration: 500, easing: 'out' },
            vAxis: { minValue: 0 }
        }

        this.chart = new google.visualization.AreaChart(chartObj)
        this.dataTable = new google.visualization.DataTable()
        this.dataTable.addColumn('string', 'string')
        this.dataTable.addColumn('number', 'Time Taken (seconds)')
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