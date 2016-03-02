(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('modalService', modalService);

    modalService.$inject = ['$uibModal'];

    function modalService($uibModal) {
        var service = {
            newModal: newModal
        };

        return service;

        function newModal(type, location, data, size, success) {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: '/app/areas/' + location + '/' + type + '.html',
                controller: type + 'Modal',
                controllerAs: 'mVm',
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
                }
            );
        }
    }
})();