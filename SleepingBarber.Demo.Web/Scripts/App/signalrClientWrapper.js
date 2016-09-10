
$(document).ready(function () {
    //$.connection.hub.logging = true;

    var barber = $.connection.barberHub;
    barber.client.customerServed = function (id) {
        OnBarberEvent('ok', id);
    };

    barber.client.failedToServe = function (id) {
        OnBarberEvent('error', id);
    };

    barber.connection.start().done(function () {
        //barber.server.reportIamListening("", "");
    });

    function OnBarberEvent(status, id) {
        var evt = $.Event('barberEvent');
        evt.status = status;
        evt.id = id;
        $(window).trigger(evt);
    }
});
