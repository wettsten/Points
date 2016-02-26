'use strict';
app.controller('newPlanningTaskModal', ['$scope', '$uibModalInstance', 'data', 'resourceService', '$timeout', function ($scope, $uibModalInstance, data, resourceService, $timeout) {

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
    $scope.enums = {
        dTypes: [],
        dUnits: [],
        fTypes: [],
        fUnits: []
    };

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
        resourceService.get('availabletasks', function (data) {
            $scope.cats = data;
            resetDropdowns();
        });
        resourceService.get('enums', function (data) {
            $scope.enums = data;
            resetDropdowns();
        });
    };

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

    $scope.confirm = function () {
        $scope.addTaskData.name = $scope.addTaskData.task.name;
        resourceService.add('planningtasks', $scope.addTaskData).then(
            function (response) {
                $uibModalInstance.close($scope.addTaskData);
            },
            function (err) {
                $scope.addError(err.data.message);
            }
        );
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    loadCats();
}]);