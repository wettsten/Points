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

    $scope.resetDropdowns = function () {
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
        resourceService.get('availabletasks');
        resourceService.get('enums');
    };

    resourceService.registerForUpdates('availabletasks', function (data) {
        $scope.cats = data;
        $scope.resetDropdowns();
    });

    resourceService.registerForUpdates('enums', function (data) {
        $scope.enums = data;
        $scope.resetDropdowns();
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
        $scope.addTaskData.name = $scope.addTaskData.task.name;
        resourceService.add('planningtasks', $scope.addTaskData).then(
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