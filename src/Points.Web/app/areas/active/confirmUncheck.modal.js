(function() {
    'use strict';

    angular
        .module('checkpoint')
        .controller('confirmUncheckModal', confirmUncheckModal);

    confirmUncheckModal.$inject = ['$uibModalInstance', 'data'];

    function confirmUncheckModal($uibModalInstance, data) {
        /* jshint validthis:true */
        var cuVm = this;

        cuVm.item = data;
        cuVm.confirm = confirm;
        cuVm.cancel = cancel;

        activate();

        function activate() { }

        function confirm () {
            $uibModalInstance.close(data.id);
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }
    }

})();