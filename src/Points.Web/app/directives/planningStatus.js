'use strict';
app.directive('planningStatus', function() {
    return {
        scope: {},
        templateUrl: '/app/views/directives/planningStatus.html',
        replace: true,
        controller: 'planningStatusController'
    };
}).controller('planningStatusController', ['$scope', '$interval', 'jobsService', '$timeout', function($scope, $interval, jobsService, $timeout) {

    $scope.render = true;
    $scope.screen = {
        left: window.innerWidth - 250,
        top: 50,
        width: 250
    };
    $scope.status = {
        title: 'Planning Countdown',
        isOpen: true,
        time: '',
        diff: '',
        color: 'panel-success'
    };

    $scope.loadData = function() {
        jobsService.getStartJob().then(
            function (result) {
                var now = new Date();
                var then = new Date(result.data.trigger);
                var diff = then - now;

                var days = Math.floor(diff / (1000 * 60 * 60 * 24));
                diff -= days * (1000 * 60 * 60 * 24);

                var hours = Math.floor(diff / (1000 * 60 * 60));
                diff -= hours * (1000 * 60 * 60);

                var mins = Math.floor(diff / (1000 * 60));

                $scope.status.diff = days + ' days, ' + hours + ' hours, ' + mins + ' minutes';
                $scope.status.time = then;
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
            });
    };

    $scope.interval = $interval(function() {
        $scope.loadData();
    }, 30000);

    $scope.loadData();
}]);