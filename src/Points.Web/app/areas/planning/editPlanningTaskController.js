'use strict';
app.controller('editPlanningTaskController', function ($scope, $uibModalInstance, task, enums) {

    $scope.enums = enums;
    $scope.task = task;

    $scope.confirm = function () {
        $uibModalInstance.close($scope.task);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.showAddDuration = function () {
        if (!$scope.task.duration.type) {
            return false;
        }
        return $scope.task.duration.type.id !== 'None';
    };

    $scope.showAddFrequency = function () {
        if (!$scope.task.frequency.type) {
            return false;
        }
        return $scope.task.frequency.type.id !== 'Once';
    };
});