'use strict';
app.controller('confirmDeleteModal', ['$scope', '$uibModalInstance', 'data', function ($scope, $uibModalInstance, data) {

    $scope.item = data;

    $scope.confirm = function () {
        $uibModalInstance.close(data.id);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}]);