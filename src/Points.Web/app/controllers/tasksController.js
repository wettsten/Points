'use strict';
app.controller('tasksController', [
    '$scope', 'tasksService', 'catsService', 'authService', function($scope, tasksService, catsService, authService) {

    $scope.tasks = [];
    $scope.message = '';
    $scope.enums = {};
    $scope.cats = [];

    $scope.loadCats = function () {
        catsService.getCats().then(function (results) {
            $scope.cats = results.data;
            $scope.$broadcast("catsLoaded", $scope.cats);
            $scope.getEnums();
        }, function (error) {
            //alert(error.data.message);
        });
    };

    $scope.getEnums = function() {
        tasksService.getEnums().then(function(results) {
            $scope.enums = results.data;
            $scope.$broadcast("enumsLoaded", $scope.enums);
            $scope.loadTasks();
        }, function(error) {
            //alert(error.data.message);
        });
    };

    $scope.loadTasks = function () {
        tasksService.getTasksByUser(authService.authentication.userId).then(function (results) {
            $scope.tasks = results.data;
            for (var i = 0; i < $scope.tasks.length; i++) {
                $scope.lookupCategory($scope.tasks[i]);
                $scope.lookupDType($scope.tasks[i]);
                $scope.lookupDUnit($scope.tasks[i]);
                $scope.lookupFType($scope.tasks[i]);
                $scope.lookupFUnit($scope.tasks[i]);
            }
            //$scope.$apply();
        }, function (error) {
            //alert(error.data.message);
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

    $scope.lookupDType = function (task) {
        for (var i = 0; i < $scope.enums.dTypes.length; i++) {
            if ($scope.enums.dTypes[i].id === task.duration.type) {
                task.duration.dType = $scope.enums.dTypes[i];
                break;
            }
        }
    };

    $scope.lookupDUnit = function (task) {
        for (var i = 0; i < $scope.enums.dUnits.length; i++) {
            if ($scope.enums.dUnits[i].id === task.duration.unit) {
                task.duration.dUnit = $scope.enums.dUnits[i];
                break;
            }
        }
    };

    $scope.lookupFType = function (task) {
        for (var i = 0; i < $scope.enums.fTypes.length; i++) {
            if ($scope.enums.fTypes[i].id === task.frequency.type) {
                task.frequency.fType = $scope.enums.fTypes[i];
                break;
            }
        }
    };

    $scope.lookupFUnit = function (task) {
        for (var i = 0; i < $scope.enums.fUnits.length; i++) {
            if ($scope.enums.fUnits[i].id === task.frequency.unit) {
                task.frequency.fUnit = $scope.enums.fUnits[i];
                break;
            }
        }
    };

    $scope.loadCats();
}]);