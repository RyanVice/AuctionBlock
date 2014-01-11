/// <reference path="_references.js" />
$(document).ready(function() {
    var viewModel = {
        firstName: ko.observable("Planet"),
        lastName: ko.observable("Earth")
    };
    viewModel.fullName = ko.dependentObservable(function() {
        // Knockout tracks dependencies automatically. It knows that fullName depends on firstName and lastName, because these get called when evaluating fullName.
        return viewModel.firstName() + " " + viewModel.lastName();
    });

    function hellowWorldViewModel() {
        var self = this;

        self.firstName = ko.observable("Planets");
        self.lastName = ko.observable("Earth");
        self.fullName = ko.dependentObservable(function () {
        // Knockout tracks dependencies automatically. It knows that fullName depends on firstName and lastName, because these get called when evaluating fullName.
        return self.firstName() + " " + self.lastName();
    });
    }

    ko.applyBindings(new hellowWorldViewModel()); // This makes Knockout get to work
});