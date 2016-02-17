'use strict';
app.controller('confirmDeleteController', function ($scope, $uibModalInstance, item) {

    $scope.item = item;

    $scope.confirm = function () {
        $uibModalInstance.close(item.id);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});