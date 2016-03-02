(function() {
    'use strict';

    angular
        .module('checkpoint')
        .controller('confirmDeleteModal', confirmDeleteModal);

    confirmDeleteModal.$inject = ['$uibModalInstance', 'data'];

    function confirmDeleteModal($uibModalInstance, data) {
        /* jshint validthis:true */
        var cdmVm = this;

        cdmVm.item = data;
        cdmVm.confirm = confirm;
        cdmVm.cancel = cancel;

        activate();

        function activate() { }

        function confirm () {
            $uibModalInstance.close(data.id);
        }

        function cancel () {
            $uibModalInstance.dismiss('cancel');
        }
    }

})();