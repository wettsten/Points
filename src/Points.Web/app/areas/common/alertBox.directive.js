(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('alertBox', alertBox);

    function alertBox() {
        var directive = {
            restrict: 'EA',
            scope: false,
            templateUrl: '/app/areas/common/alertBox.html',
            controller: 'alertBoxController',
            controllerAs: 'abVm'
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('alertBoxController', alertBoxController);

    alertBoxController.$inject = ['$timeout'];

    function alertBoxController($timeout) {
        /* jshint validthis:true */
        var abVm = this;

        abVm.alerts = [];
        abVm.addSuccess = addSuccess;
        abVm.addWarning = addWarning;
        abVm.addError = addError;
        abVm.closeAlert = closeAlert;

        activate();

        function activate() { }

        function addAlert (type, msg) {
            var alert = { type: type, msg: msg };
            abVm.alerts.push(alert);
            $timeout(function () {
                closeAlert(alert);
            }, 5000);
        }

        function addSuccess (msg) {
            addAlert('success', msg);
        }

        function addWarning (msg) {
            addAlert('warning', msg);
        }

        function addError(msg) {
            addAlert('danger', msg);
        }

        function closeAlert (alert) {
            if (abVm.alerts.indexOf(alert) > -1) {
                abVm.alerts.splice(abVm.alerts.indexOf(alert), 1);
            }
        }
    }

})();