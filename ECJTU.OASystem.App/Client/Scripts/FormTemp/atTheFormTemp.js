(function () {
    var app = angular.module("atTheFormTemp", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/formTemp/list", { templateUrl: "../client/views/formTemp/list.html" })
            .otherwise({ redirectTo: "/formTemp/list" });
    }
    app.config(config);
    app.constant("formTempApiUrl", "../formTemp/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());