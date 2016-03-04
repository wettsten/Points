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
                templateUrl: '/app/areas/home/home.html'
            })
            .when('/login', {
                controller: 'loginController',
                controllerAs: 'loginVm',
                templateUrl: '/app/areas/login/login.html'
            })
            .when('/signup', {
                controller: 'signupController',
                controllerAs: 'signupVm',
                templateUrl: '/app/areas/login/signup.html'
            })
            .when('/active', {
                controller: 'activeController',
                controllerAs: 'activeVm',
                templateUrl: '/app/areas/active/active.html'
            })
            .when('/planning', {
                controller: 'planningController',
                controllerAs: 'planningVm',
                templateUrl: '/app/areas/planning/planning.html'
            })
            .when('/cats', {
                controller: 'catsController',
                controllerAs: 'catsVm',
                templateUrl: '/app/areas/categories/cats.html'
            })
            .when('/tasks', {
                controller: 'tasksController',
                controllerAs: 'tasksVm',
                templateUrl: '/app/areas/tasks/tasks.html'
            })
            .when('/options', {
                controller: 'optionsController',
                controllerAs: 'optionsVm',
                templateUrl: '/app/areas/options/options.html'
            })
            .when('/associate', {
                controller: 'associateController',
                controllerAs: 'associateVm',
                templateUrl: '/app/areas/login/associate.html'
            })
            .otherwise({ redirectTo: '/home' });

        $httpProvider.interceptors.push('authInterceptorService');
    }

})();
