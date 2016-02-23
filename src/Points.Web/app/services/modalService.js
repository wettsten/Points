'use strict';
app.factory('modalService', ['$uibModal', function ($uibModal) {

    var modalService = {};

    modalService.newModal = function(type, data, size, success) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/app/views/partials/' + type + '.html',
            controller: type + 'Modal',
            size: size,
            resolve: {
                data: data
            }
        });
        modalInstance.result.then(
            function (result) {
                if (result) {
                    success(result);
                }
            });
    };

    return modalService;

}]);