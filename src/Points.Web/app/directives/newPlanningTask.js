'use strict';
app.directive('newPlanningTask', function () {
    return {
        scope: {
            addAlert: '&'
        },
        templateUrl: '/app/views/directives/newPlanningTask.html',
        replace: true,
        controller: 'newPlanningTaskController'
    };
}).controller('newPlanningTaskController', ['$scope', 'resourceService', '$timeout', function ($scope, resourceService, $timeout) {
        
    $scope.addTaskData = {
        duration: {
            value: 0,
            type: {},
            unit: {}
        },
        frequency: {
            value: 1,
            type: {},
            unit: {}
        },
        task: {},
        cat: {}
    };
    $scope.cats = [];
    $scope.enums = {};

    var resetDropdowns = function () {
        $scope.addTaskData.cat = $scope.cats[0];
        if ($scope.cats.length > 0) {
            $scope.addTaskData.task = $scope.cats[0].tasks[0];
        }
        $scope.addTaskData.duration.type = $scope.enums.dTypes[0];
        $scope.addTaskData.duration.unit = $scope.enums.dUnits[0];
        $scope.addTaskData.frequency.type = $scope.enums.fTypes[0];
        $scope.addTaskData.frequency.unit = $scope.enums.fUnits[0];
    };

    var loadCats = function () {
        $scope.cats = resourceService.get('availabletasks');
        $scope.enums = resourceService.get('enums');
        $timeout(function() {
            resetDropdowns();
        }, 1000);
    };

    resourceService.registerForUpdates('availabletasks', function (data) {
        $scope.cats = data;
        resetDropdowns();
    });

    resourceService.registerForUpdates('enums', function (data) {
        $scope.enums = data;
        resetDropdowns();
    });

    $scope.showAddDuration = function () {
        if (!$scope.addTaskData.duration.type) {
            return false;
        }
        return $scope.addTaskData.duration.type.id !== 'None';
    };

    $scope.showAddFrequency = function () {
        if (!$scope.addTaskData.frequency.type) {
            return false;
        }
        return $scope.addTaskData.frequency.type.id !== 'Once';
    };

    $scope.addTask = function () {
        var aTask = angular.copy($scope.addTaskData);
        aTask.name = $scope.addTaskData.task.id;
        aTask.taskId = $scope.addTaskData.task.id;
        aTask.duration.type = $scope.addTaskData.duration.type.id;
        aTask.duration.unit = $scope.addTaskData.duration.unit.id;
        aTask.frequency.type = $scope.addTaskData.frequency.type.id;
        aTask.frequency.unit = $scope.addTaskData.frequency.unit.id;
        resourceService.add('planningtasks',aTask).then(
            function (response) {
                $scope.resetAddData();
                $scope.addAlert({ type: 'success', msg: 'Task successfully added' });
            },
            function (err) {
                $scope.addAlert({ type: 'danger', msg: err.data.message });
         });
    };

    loadCats();
}]);