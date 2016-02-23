'use strict';
app.directive('planningStatus', function() {
    return {
        scope: {},
        templateUrl: '/app/views/directives/planningStatus.html',
        replace: true,
        controller: 'planningStatusController'
    };
}).controller('planningStatusController', ['$scope', '$interval', 'jobsService', '$timeout', 'planningTasksService', '$q', function ($scope, $interval, jobsService, $timeout, planningTasksService, $q) {

    $scope.render = true;
    $scope.screen = {
        left: window.innerWidth - 300,
        top: 50,
        width: 300
    };
    $scope.status = {
        title: 'Planning Countdown',
        isOpen: false,
        time: '',
        diff: '',
        color: 'panel-success'
    };

    $scope.getStartJob = jobsService.getStartJob().then(
        function (results) {
            return results.data;
        });

    $scope.loadPlanningTasks = planningTasksService.getTasks().then(
        function (results) {
            return results.data;
        });

    $scope.loadData = function() {
        $q.all([$scope.getStartJob, $scope.loadPlanningTasks]).then(
            function (data) {
                var days = $scope.calculateDate(data[0].trigger);
                $scope.updatePanelColor(days);
                $scope.status.title = 'Planning Countdown (' + data[1].length + ')';
            });
    };

    $scope.calculateDate = function(trigger) {
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

    $scope.updatePanelColor = function(days) {
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