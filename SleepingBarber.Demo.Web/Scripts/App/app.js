angular.module("demo", []);

angular.module("demo").factory("customersService", function ($http) {
    var service = {}
    service.sendToQueue = function (count, callback) {
        $http.post('/Barber/Create', { customers: count }).then(function (result) {
            callback(result.data);
        });
    };
    return service;
});

angular.module("demo").controller("demoController", function ($scope, customersService) {
    $scope.events = [];
    $scope.successList = [];
    $scope.failList = [];
    $scope.customers = { count: 2 };
    $scope.addCustomers = function () {
        var addToEvents = function (list) {
            $scope.events = _.union($scope.events, list);
        };
        customersService.sendToQueue($scope.customers.count, addToEvents);
    };
    $scope.clearEvents = function () {
        $scope.events = [];
        $scope.successList = [];
        $scope.failList = [];
    };

    $(window).on('barberEvent', function (e) {
        if (e.status == 'ok') {
            $scope.successList.push(e.id);
        }
        if (e.status == 'error') {
            $scope.failList.push(e.id);
        }
        $scope.$apply();
    });

});