'use strict';
app.controller('tasksController', [
    '$scope', 'tasksService', 'catsService', 'filterFactory', '$timeout', '$q', function ($scope, tasksService, catsService, filterFactory, $timeout, $q) {

    $scope.tasks = [];
    $scope.alerts = [];
    $scope.taskInEdit = { id: '' };
    $scope.taskFilter = filterFactory.getTaskFilter();

    $scope.loadCats = catsService.getCats().then(
        function (results) {
            return results.data;
        });

    $scope.loadTasks = tasksService.getTasks().then(
        function (results) {
            return results.data;
        });

    $scope.loadData = function () {
        $q.all([$scope.loadCats, $scope.loadTasks]).then(
            function (data) {
                $scope.cats = data[0];
                $scope.tasks = data[1];
                for (var i = 0; i < $scope.tasks.length; i++) {
                    $scope.lookupCategory($scope.tasks[i]);
                }
                if ($scope.tasks.length === 0) {
                    $scope.addAlert('warning', 'No tasks found');
                }
            });
    };

    $scope.lookupCategory = function(task) {
        for (var i = 0; i < $scope.cats.length; i++) {
            if ($scope.cats[i].id === task.categoryId) {
                task.category = $scope.cats[i];
                break;
            }
        }
    };

    filterFactory.subscribe($scope, 'taskFilter', function taskFilterChanged() {
        $scope.taskFilter = filterFactory.getTaskFilter();
    });

    $scope.$on('refreshTasks', function () {
        $scope.loadData();
    });

    $scope.addAlert = function (type, msg) {
        var alert = { type: type, msg: msg };
        $scope.alerts.push(alert);
        $timeout(function () {
            if ($scope.alerts.indexOf(alert) > -1) {
                $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
            }
        }, 5000);
    };

    $scope.closeAlert = function (alert) {
        if ($scope.alerts.indexOf(alert) > -1) {
            $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
        }
    };

    $scope.loadData();
}]);