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
            templateUrl: '/app/areas/jplanning/planningTaskCard.html',
            controller: 'planningTaskCardController',
            controllerAs: 'pCardVm'
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('planningTaskCardController', planningTaskCardController);

    planningTaskCardController.$inject = ['resourceService', 'modalService'];

    function planningTaskCardController(resourceService, modalService) {
        /* jshint validthis:true */
        var pCardVm = this;

        pCardVm.enums = {};
        pCardVm.startEdit = startEdit;
        pCardVm.delete = deletePlanningTask;

        activate();

        function activate() {
            resourceService.get('enums', function (data) {
                pCardVm.enums = data;
            });
        }

        function startEdit () {
            modalService.newModal('editPlanningTask', angular.copy(pCardVm.task), 'lg',
                function (result) {
                    pCardVm.addSuccess({ msg: "Task '{0}' successfully updated".format(result.name) });
                }
            );
        }

        function deletePlanningTask() {
            var name = pCardVm.task.name;
            modalService.newModal('confirmDelete', { name: pCardVm.task.name, id: pCardVm.task.id }, 'sm',
                function (result) {
                    resourceService.delete('planningtasks', pCardVm.task.id).then(
                        function (response) {
                            pCardVm.addSuccess({ msg: "Task '{0}' successfully deleted".format(name) });
                        },
                        function (err) {
                            pCardVm.addError({ msg: err.data.message });
                        }
                    );
                }
            );
        }
    }

})();