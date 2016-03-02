(function () {
    'use strict';

    angular
        .module('checkpoint')
        .config(config);

    config.$inject = ['$routeProvider', '$httpProvider'];

    function config($routeProvider, $httpProvider) {
        $routeProvider
            .when('/home', {
                controller: 'homeController',
                controllerAs: 'homeVm',
                templateUrl: '/app/views/home.html'
            })
            .when('/login', {
                controller: 'loginController',
                controllerAs: 'loginVm',
                templateUrl: '/app/views/login.html'
            })
            .when('/signup', {
                controller: 'signupController',
                controllerAs: 'signupVm',
                templateUrl: '/app/views/signup.html'
            })
            .when('/active', {
                controller: 'activeController',
                controllerAs: 'activeVm',
                templateUrl: '/app/views/active.html'
            })
            .when('/planning', {
                controller: 'planningController',
                controllerAs: 'planningVm',
                templateUrl: '/app/views/planning.html'
            })
            .when('/cats', {
                controller: 'catsController',
                controllerAs: 'catsVm',
                templateUrl: '/app/views/cats.html'
            })
            .when('/tasks', {
                controller: 'tasksController',
                controllerAs: 'tasksVm',
                templateUrl: '/app/views/tasks.html'
            })
            .when('/options', {
                controller: 'optionsController',
                controllerAs: 'optionsVm',
                templateUrl: '/app/views/options.html'
            })
            .when('/associate', {
                controller: 'associateController',
                controllerAs: 'associateVm',
                templateUrl: '/app/views/associate.html'
            })
            .otherwise({ redirectTo: '/home' });

        $httpProvider.interceptors.push('authInterceptorService');
    }

})();
