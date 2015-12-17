'use strict';
app.controller('tasksController', [
    '$scope', 'tasksService', 'catsService', 'filterFactory', '$timeout', function ($scope, tasksService, catsService, filterFactory, $timeout) {

    $scope.tasks = [];
    $scope.alerts = [];
    $scope.taskInEdit = { id: '' };
    $scope.taskFilter = filterFactory.getTaskFilter();

    $scope.loadCats = function () {
        catsService.getCats().then(function (results) {
            $scope.cats = results.data;
            $scope.loadTasks();
        }, function (err) {
            $scope.addAlert('danger', err.statusText);
        });
    };

    $scope.loadTasks = function () {
        tasksService.getTasks().then(function (results) {
            $scope.tasks = results.data;
            for (var i = 0; i < $scope.tasks.length; i++) {
                $scope.lookupCategory($scope.tasks[i]);
            }
        }, function (err) {
            $scope.addAlert('danger', err.statusText);
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
        $scope.loadCats();
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

    $scope.loadCats();
}]);