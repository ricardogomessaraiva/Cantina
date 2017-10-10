var ComingSoon = function () {

    return {
        //main function to initiate the module
        init: function () {
            var austDay = new Date();
            austDay.setHours(austDay.getHours() + 48);
            austDay = new Date(austDay);
            $('#defaultCountdown').countdown({ until: austDay });
            $('#year').text(austDay.getFullYear());
        }

    };

}();