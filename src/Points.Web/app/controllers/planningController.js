'use strict';
app.controller('planningController', ['$scope', 'catsService', 'planningTasksService', '$timeout', '$q', function ($scope, catsService, planningTasksService, $timeout, $q) {

    $scope.tasks = [];
    $scope.cats = [];
    $scope.alerts = [];
    $scope.filteredCats = [];
    $scope.taskInEdit = { id: '' };

    $scope.loadCats = catsService.getCats().then(
        function (results) {
            return results.data;
        });

    $scope.loadTasks = planningTasksService.getTasks().then(
        function (results) {
            return results.data;
        });

    $scope.loadData = function () {
        $q.all([$scope.loadCats, $scope.loadTasks]).then(
            function (data) {
                $scope.cats = data[0];
                for (var i = 0; i < $scope.cats.length; i++) {
                    $scope.cats[i].tasks = [];
                    $scope.cats[i].isOpen = false;
                }
                if ($scope.cats.length > 0) {
                    $scope.cats[0].isOpen = true;
                }
                $scope.tasks = data[1];
                for (var i = 0; i < $scope.tasks.length; i++) {
                    $scope.lookupCategory($scope.tasks[i]);
                }
                $scope.filterCats();
                if ($scope.tasks.length === 0) {
                    $scope.addAlert('warning', 'No planning tasks found');
                }
            });
    };

    $scope.filterCats = function() {
        $scope.filteredCats = $scope.cats.filter(
        function (cat) {
            return cat.tasks.length > 0;
        });
    };

    $scope.lookupCategory = function (task) {
        for (var i = 0; i < $scope.cats.length; i++) {
            if ($scope.cats[i].id === task.task.category.id) {
                $scope.cats[i].tasks.push(task);
                break;
            }
        }
    };

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