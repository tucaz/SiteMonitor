var util = {
    querystring: function (name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.search);
        if (results == null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}

google.load('visualization', '1.0', { 'packages': ['corechart'] });
google.setOnLoadCallback(Initialize);

function Initialize() {
    var runnerName = util.querystring('runnerName'),
        from = util.querystring('from'),
        to = util.querystring('to'),
        interval = util.querystring('interval');

    LoadRunner(runnerName, from, to, interval);
}

function LoadRunner(runnerName, from, to, interval) {
    var url = '/API/Runner/GetResults?' + 
        'runnerName=' + runnerName +
        '&from=' + from +
        '&to=' + to +
        '&interval=' + interval;

    $.get(url, function (result) {
        RunnerChart.initialize(document.getElementById('chart'), result.Title, result.YAxisLegend);
        RunnerChart.render(result.Entries);
    });

    ReloadRunner();
}

function ReloadRunner() {
    setTimeout(Initialize, 60000);
}