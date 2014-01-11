/// <reference path="_references.js" />
$(function () {
    var appViewModel = (function () {
        // Private fields
        var selectedAuction = window.ko.observable(),
            newAuction = window.ko.observable(),
            editAuction = window.ko.observable(),
            auctions = window.ko.observableArray([]),
            showItems = window.ko.observable(true),
            mainViewModel = window.ko.observable(auctions),
            mainViewTemplate = window.ko.observable('auctionsView');

        // Private functions
        var toAuction = function (auctionResponse) {
            var auction = window.ko.mapping.toJS(auctionResponse);

            // Review - How would I let the binding system know this is updated?
            // Review - Should this be encapsulated in a class?
            auction.maxBid = window.ko.computed(
                function () {

                    var maxBid = 0;
                    $(auction.bids).each(function (index, bid) {
                        if (bid.amount > maxBid) {
                            maxBid = bid.amount;
                        }
                    });

                    return maxBid;
                });

            return auction;
        };

        var showAuctionsView = function () {
            mainViewModel(auctions);
            mainViewTemplate("auctionsView");
        };

        return {
            selectedAuction: selectedAuction,
            auctions: auctions,
            loadAuctions: function () {
                $.getJSON("/api/Auctions",
                    function (data) {
                        $.each(data, function (i, a) {
                            appViewModel.auctions.push(toAuction(a));
                        });
                        showItems(true);
                    });
            },
            onItemSelected: function (data) {
                mainViewModel(toAuction(data));
                mainViewTemplate("auctionDetailsView");
            },
            showItems: showItems,
            onCloseAuctionDetails: function () {
                showAuctionsView();
            },
            onAddClicked: function () {
                mainViewModel(newAuction);
                mainViewTemplate("newAuctionView");
            },
            newAuction: newAuction,
            onCancelNewAuction: function () {
                showAuctionsView();
            },
            onEditClicked: function (data) {
                mainViewModel(selectedAuction);
                mainViewTemplate("editAuctionView");
            },
            editAuction: editAuction,
            onCancelEditAuction: function () {
                showAuctionsView();
            },
            onDeleteClicked: function (data) {
                alert("Deleting " + data.title);
            },
            mainViewModel: mainViewModel,
            mainViewTemplate: mainViewTemplate
        };
    })();

    window.ko.applyBindings(appViewModel);

    appViewModel.loadAuctions();
});