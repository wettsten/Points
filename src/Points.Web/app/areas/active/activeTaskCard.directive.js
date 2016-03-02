(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('activeTaskCard', activeTaskCard);
    
    function activeTaskCard() {
        var directive = {
            restrict: 'EA',
            scope: {
                task: '=theTask',
                addSuccess: '&',
                addError: '&'
            },
            templateUrl: '/app/areas/active/activeTaskCard.html',
            controller: 'activeTaskCardController',
            controllerAs: 'aCardVm',
            bindToController: true
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('activeTaskCardController', activeTaskCardController);

    activeTaskCardController.$inject = ['resourceService', '$uibModal'];

    function activeTaskCardController(resourceService, $uibModal) {
        /* jshint validthis:true */
        var aCardVm = this;

        aCardVm.status = status;
        aCardVm.check = check;
        aCardVm.uncheck = uncheck;

        activate();

        function activate() { }

        function status () {
            var pct = aCardVm.task.timesCompleted * 100 / aCardVm.task.frequency.value;
            if (aCardVm.task.frequency.type.id === 'AtMost') {
                if (pct > 100) {
                    return 'panel-danger';
                } else if (pct > 50) {
                    return 'panel-warning';
                }
                return 'panel-success';
            }
            if (pct >= 100) {
                return 'panel-success';
            } else if (pct >= 50) {
                return 'panel-warning';
            }
            return 'panel-danger';
        }

        function check () {
            aCardVm.task.timesCompleted += 1;
            resourceService.edit('activetasks', aCardVm.task).then(
                function (response) {
                    aCardVm.addSuccess({ msg: 'Task successfully checked' });
                },
                function (err) {
                    aCardVm.addError({ msg: err.data.message });
                }
            );
        }

        function uncheck() {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: '/app/areas/active/confirmUncheck.html',
                controller: 'confirmUncheckModal',
                controllerAs: 'cuVm',
                size: 'sm',
                resolve: {
                    data: { name: aCardVm.task.name, id: aCardVm.task.id }
                }
            });
            modalInstance.result.then(
                function (result) {
                    if (result) {
                        aCardVm.task.timesCompleted -= 1;
                        resourceService.edit('activetasks', aCardVm.task).then(
                            function (response) {
                                aCardVm.addSuccess({ msg: 'Task successfully unchecked' });
                            },
                            function (err) {
                                $saCardVmcope.addError({ msg: err.data.message });
                            }
                        );
                    }
                }
            );
        }
    }

})();