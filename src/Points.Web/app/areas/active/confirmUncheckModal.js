'use strict';
app.controller('confirmUncheckModal', ['$scope', '$uibModalInstance', 'data', function ($scope, $uibModalInstance, data) {

    $scope.item = data;

    $scope.confirm = function () {
        $uibModalInstance.close(data.id);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}]);