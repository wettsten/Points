(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('planningTaskCard', planningTaskCard);
    
    function planningTaskCard() {
        var directive = {
            restrict: 'EA',
            scope: {
                task: '=theTask',
                addSuccess: '&',
                addError: '&'
            },
            templateUrl: '/app/areas/planning/planningTaskCard.html',
            controller: 'planningTaskCardController',
            controllerAs: 'pCardVm',
            bindToController: true
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('planningTaskCardController', planningTaskCardController);

    planningTaskCardController.$inject = ['resourceService', 'modalService', '$uibModal'];

    function planningTaskCardController(resourceService, modalService, $uibModal) {
        /* jshint validthis:true */
        var pCardVm = this;

        pCardVm.enums = {};
        pCardVm.startEdit = startEdit;
        pCardVm.delete = deletePlanningTask;

        activate();

        function activate() {
            resourceService.get('users/enums', function (data) {
                pCardVm.enums = data;
            });
        }

        function startEdit() {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: '/app/areas/planning/editPlanningTask.html',
                controller: 'editPlanningTaskModal',
                controllerAs: 'editPlanVm',
                size: 'lg',
                resolve: {
                    data: angular.copy(pCardVm.task)
                }
            });
            modalInstance.result.then(editModalResult);
        }

        function editModalResult(result) {
            if (result) {
                pCardVm.addSuccess({ msg: "Task '{0}' successfully updated".format(result.name) });
            }
        }

        function deletePlanningTask() {
            modalService.newModal('confirmDelete', 'common', { name: pCardVm.task.name, id: pCardVm.task.id }, 'sm', deleteModalResult);
        }

        function deleteModalResult(result) {
            resourceService
                .remove('planningtasks', pCardVm.task.id)
                .then(deleteSuccess, deleteError);
        }

        function deleteSuccess(response) {
            pCardVm.addSuccess({ msg: "Task '{0}' successfully deleted".format(pCardVm.task.name) });
        }

        function deleteError(err) {
            pCardVm.addError({ msg: err.data.message });
        }
    }

})();