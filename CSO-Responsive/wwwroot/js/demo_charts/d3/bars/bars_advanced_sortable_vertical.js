/* ------------------------------------------------------------------------------
 *
 *  # D3.js - vertical sortable bars
 *
 *  Demo d3.js vertical bar chart setup with animated sorting and .tsv data source
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var D3BarSortableVertical = function() {


    //
    // Setup module components
    //

    // Chart
    var _barSortableVertical = function() {
        if (typeof d3 == 'undefined') {
            console.warn('Warning - d3.min.js is not loaded.');
            return;
        }

        // Main variables
        var element = document.getElementById('d3-bar-sortable-vertical'),
            height = 400;


        // Initialize chart only if element exsists in the DOM
        if(element) {

            // Basic setup
            // ------------------------------

            // Define main variables
            var d3Container = d3.select(element),
                margin = {top: 5, right: 20, bottom: 20, left: 40},
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right,
                height = height - margin.top - margin.bottom - 5;

            // Format data
            var formatPercent = d3.format(".0%");

            // Colors
            var colors = d3.scale.category20c();



            // Construct scales
            // ------------------------------

            // Horizontal
            var x = d3.scale.ordinal()
                .rangeRoundBands([0, width], .1, 1);

            // Vertical
            var y = d3.scale.linear()
                .range([height, 0]);



            // Create axes
            // ------------------------------

            // Horizontal
            var xAxis = d3.svg.axis()
                .scale(x)
                .orient("bottom");

            // Vertical
            var yAxis = d3.svg.axis()
                .scale(y)
                .orient("left")
                .tickFormat(formatPercent);



            // Create chart
            // ------------------------------

            // Add SVG element
            var container = d3Container.append("svg");

            // Add SVG group
            var svg = container
                .attr("width", width + margin.left + margin.right)
                .attr("height", height + margin.top + margin.bottom)
                .append("g")
                    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


            // Load data
            // ------------------------------

            d3.tsv("../../../../demo_data/d3/bars/bars_basic.tsv", function(error, data) {

                // Pull out values
                data.forEach(function(d) {
                    d.frequency = +d.frequency;
                });


                // Set input domains
                // ------------------------------

                // Horizontal
                x.domain(data.map(function(d) { return d.letter; }));

                // Vertical
                y.domain([0, d3.max(data, function(d) { return d.frequency; })]);


                //
                // Append chart elements
                //

                // Append axes
                // ------------------------------

                // Horizontal
                svg.append("g")
                    .attr("class", "d3-axis d3-axis-horizontal")
                    .attr("transform", "translate(0," + height + ")")
                    .call(xAxis);

                // Vertical
                var verticalAxis = svg.append("g")
                    .attr("class", "d3-axis d3-axis-vertical")
                    .call(yAxis);

                // Add text label
                verticalAxis.append("text")
                    .attr("class", "d3-axis-title")
                    .attr("transform", "rotate(-90)")
                    .attr("y", 10)
                    .attr("dy", ".71em")
                    .style("text-anchor", "end")
                    .text("Frequency");


                // Append bars
                // ------------------------------

                svg.selectAll(".d3-bar")
                    .data(data)
                    .enter()
                    .append("rect")
                        .attr("class", "d3-bar")
                        .attr("fill", function(d, i) { return colors(i); })
                        .attr("x", function(d) { return x(d.letter); })
                        .attr("width", x.rangeBand())
                        .attr("y", function(d) { return y(d.frequency); })
                        .attr("height", function(d) { return height - y(d.frequency); });


                // Change data sets
                // ------------------------------

                // Attach change event
                d3.select(".toggle-sort").on("change", change);

                // Sort values on page load with delay
                var sortTimeout = setTimeout(function() {
                    d3.select(".toggle-sort").property("checked", true).each(change);
                }, 2000);

                // Sorting function
                function change() {
                    clearTimeout(sortTimeout);

                    // Copy-on-write since tweens are evaluated after a delay.
                    var x0 = x.domain(data.sort(this.checked
                        ? function(a, b) { return b.frequency - a.frequency; }
                        : function(a, b) { return d3.ascending(a.letter, b.letter); })
                        .map(function(d) { return d.letter; }))
                        .copy();

                    var transition = svg.transition().duration(750),
                        delay = function(d, i) { return i * 50; };

                    transition.selectAll(".d3-bar")
                        .delay(delay)
                        .attr("x", function(d) { return x0(d.letter); });

                    transition.select(".d3-axis-horizontal")
                        .call(xAxis)
                        .selectAll("g")
                        .delay(delay);
                }
            });



            // Resize chart
            // ------------------------------

            // Call function on window resize
            window.addEventListener('resize', resize);

            // Call function on sidebar width change
            var sidebarToggle = document.querySelector('.sidebar-control');
            sidebarToggle && sidebarToggle.addEventListener('click', resize);

            // Resize function
            // 
            // Since D3 doesn't support SVG resize by default,
            // we need to manually specify parts of the graph that need to 
            // be updated on window resize
            function resize() {

                // Layout variables
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right;


                // Layout
                // -------------------------

                // Main svg width
                container.attr("width", width + margin.left + margin.right);

                // Width of appended group
                svg.attr("width", width + margin.left + margin.right);


                // Axes
                // -------------------------

                // Horizontal range
                x.rangeRoundBands([0, width], .1, 1);

                // Horizontal axis
                svg.selectAll('.d3-axis-horizontal').call(xAxis);


                // Chart elements
                // -------------------------

                // Line path
                svg.selectAll('.d3-bar').attr("width", x.rangeBand()).attr("x", function(d) { return x(d.letter); });
            }
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _barSortableVertical();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    D3BarSortableVertical.init();
});
