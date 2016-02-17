'use strict';
app.directive('activeStatus', function() {
    return {
        scope: {},
        templateUrl: '/app/views/directives/activeStatus.html',
        replace: true,
        controller: 'activeStatusController'
    };
}).controller('activeStatusController', ['$scope', '$interval', 'jobsService', '$timeout', '$q', 'activeTasksService', function ($scope, $interval, jobsService, $timeout, $q, activeTasksService) {

    $scope.render = true;
    $scope.screen = {
        left: 0,
        top: 50,
        width: 300
    };
    $scope.status = {
        title: 'Active Countdown',
        isOpen: false,
        time: '',
        diff: '',
        color: 'panel-success'
    };

    $scope.getEndJob = jobsService.getEndJob().then(
        function (results) {
            return results.data;
        });

    $scope.loadActiveTasks = activeTasksService.getTasks().then(
        function (results) {
            return results.data;
        });

    $scope.loadData = function() {
        $q.all([$scope.getEndJob, $scope.loadActiveTasks]).then(
            function (data) {
                var days = $scope.calculateDate(data[0].trigger);
                $scope.updatePanelColor(days);
                var completed = data[1].filter(
                    function (task) {
                        return task.isCompleted;
                    });
                $scope.status.title = 'Active Countdown (' + completed.length + '/' + data[1].length + ')';
            });
    };

    $scope.calculateDate = function (trigger) {
        var now = new Date();
        var then = new Date(trigger);
        var diff = then - now;

        var days = Math.floor(diff / (1000 * 60 * 60 * 24));
        diff -= days * (1000 * 60 * 60 * 24);

        var hours = Math.floor(diff / (1000 * 60 * 60));
        diff -= hours * (1000 * 60 * 60);

        var mins = Math.floor(diff / (1000 * 60));

        $scope.status.diff = days + ' days, ' + hours + ' hours, ' + mins + ' minutes';
        $scope.status.time = then;
        return days;
    };

    $scope.updatePanelColor = function (days) {
        if (days < 1) {
            if ($scope.status.color !== 'panel-danger') {
                $scope.render = false;
                $scope.status.color = 'panel-danger';
                $timeout(function () {
                    $scope.render = true;
                });
            }
        } else if (days < 4) {
            if ($scope.status.color !== 'panel-warning') {
                $scope.render = false;
                $scope.status.color = 'panel-warning';
                $timeout(function () {
                    $scope.render = true;
                });
            }
        } else {
            if ($scope.status.color !== 'panel-success') {
                $scope.render = false;
                $scope.status.color = 'panel-success';
                $timeout(function () {
                    $scope.render = true;
                });
            }
        }
    };

    $scope.interval = $interval(function() {
        $scope.loadData();
    }, 30000);

    $scope.loadData();
}]);