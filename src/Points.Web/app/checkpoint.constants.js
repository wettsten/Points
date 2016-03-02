(function () {
    'use strict';

    angular
        .module('cp.core')
        .constant('cpSettings', {
            apiServiceBaseUri: 'http://points.wettsten.com/auth/',
            apiResourceBaseUri: 'http://points.wettsten.com/resources/',
            clientId: 'checkpoint'
        });

})();
