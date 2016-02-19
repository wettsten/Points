'use strict';
app.controller('editPlanningTaskModal', ['$scope', '$uibModalInstance', 'data', 'resourceService', function ($scope, $uibModalInstance, data, resourceService) {

    $scope.enums = {
        dTypes: [],
        dUnits: [],
        fTypes: [],
        fUnits: []
    };
    $scope.task = data;

    var loadEnums = function () {
        resourceService.get('enums');
    };

    resourceService.registerForUpdates('enums', function (data) {
        $scope.enums = data;
    });

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

    loadEnums();
}]);