var searchUrl = searchUrl || "/Search/Submit";
let searchTimerId = null;

const app = new Vue({
    el: "#v_app",
    data: {
        query: "",
        results: [],
        resultsEmpty: false,
        next: null,
        searching: false
    },
    watch: {
        query: function() {
            if (searchTimerId) {
                clearTimeout(searchTimerId);
            }

            this.results = [];
            this.resultsEmpty = false;

            searchTimerId = setTimeout(() => {
                this.search();
            }, 800);
        }
    },
    methods: {
        getCommaSeperated: function (listItems) {
            if (typeof listItems === "object") {
                listItems = Object.values(listItems);
            }

            if (listItems == null || listItems.length === 0) {
                return "";
            }
            
            var response = "";

            for (let i = 0; i < listItems.length; i++) {
                response += `"${listItems[i]}"`;

                if (i + 2 === listItems.length) {
                    response += " and ";
                } else if (i + 1 < listItems.length) {
                    response += ", ";
                } 
            }

            return response;
        },
        search: function () {
            if (this.query.length === 0 || this.searching) {
                return;
            }

            this.searching = true;

            if (searchTimerId) {
                clearTimeout(searchTimerId);
                searchTimerId = null;
            }

            this.results = [];
            this.resultsEmpty = false;
            this.next = null;

            fetch(searchUrl + "?q=" + encodeURIComponent(this.query))
                .then(res => res.json())
                .then(res => {
                    this.searching = false;
                    this.results = res.results;
                    this.resultsEmpty = this.results.length === 0;
                    this.next = res.next;
                });
        }
    }
});