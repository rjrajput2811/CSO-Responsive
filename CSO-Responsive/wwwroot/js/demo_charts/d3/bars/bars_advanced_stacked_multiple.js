/* ------------------------------------------------------------------------------
 *
 *  # D3.js - stacked and multiple bars
 *
 *  Demo d3.js bar chart setup with animated transition between stacked and multiple bars
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var D3BarStackedMultiple = function() {


    //
    // Setup module components
    //

    // Chart
    var _barStackedMultiple = function() {
        if (typeof d3 == 'undefined') {
            console.warn('Warning - d3.min.js is not loaded.');
            return;
        }

        // Main variables
        var element = document.getElementById('d3-bar-stacked-multiples'),
            height = 400;


        // Initialize chart only if element exsists in the DOM
        if(element) {

            // Basic setup
            // ------------------------------

            // Define main variables
            var d3Container = d3.select(element),
                margin = {top: 5, right: 20, bottom: 20, left: 60},
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right,
                height = height - margin.top - margin.bottom - 5;

            // Format data
            var parseDate = d3.time.format("%Y-%m").parse,
                formatYear = d3.format("02d"),
                formatDate = function(d) { return "Q" + ((d.getMonth() / 3 | 0) + 1) + formatYear(d.getFullYear() % 100); };

            // Colors
            var color = d3.scale.category20();



            // Construct scales
            // ------------------------------

            // Horizontal
            var x = d3.scale.ordinal()
                .rangeRoundBands([0, width], .2);

            // Vertical
            var y = d3.scale.ordinal()
                .rangeRoundBands([height, 0]);

            var y0 = d3.scale.ordinal()
                .rangeRoundBands([height, 0]);

            var y1 = d3.scale.linear();



            // Create axes
            // ------------------------------

            // Horizontal
            var xAxis = d3.svg.axis()
                .scale(x)
                .orient("bottom")
                .tickFormat(formatDate);

            // Vertical
            var yAxis = d3.svg.axis()
                .scale(y)
                .orient("left")
                .ticks(10, "%");



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



            // Construct chart layout
            // ------------------------------

            // Nest
            var nest = d3.nest()
                .key(function(d) { return d.browser; });

            // Stack
            var stack = d3.layout.stack()
                .values(function(d) { return d.values; })
                .x(function(d) { return d.date; })
                .y(function(d) { return d.value; })
                .out(function(d, y0) { d.valueOffset = y0; });




            // Load data
            // ------------------------------

            d3.tsv("../../../../demo_data/d3/bars/bars_stacked_multiple.tsv", function(error, data) {

                // Pull out values
                data.forEach(function(d) {
                    d.date = parseDate(d.date);
                    d.value = +d.value;
                });

                // Nest values
                var dataByGroup = nest.entries(data);


                // Set input domains
                // ------------------------------

                // Stack
                stack(dataByGroup);

                // Horizontal
                x.domain(dataByGroup[0].values.map(function(d) { return d.date; }));

                // Vertical
                y0.domain(dataByGroup.map(function(d) { return d.key; }));
                y1.domain([0, d3.max(data, function(d) { return d.value; })]).range([y0.rangeBand(), 0]);


                //
                // Append chart elements
                //

                // Add bars
                // ------------------------------

                // Group bars
                var group = svg.selectAll(".d3-bar-group")
                    .data(dataByGroup)
                    .enter()
                    .append("g")
                        .attr("class", "d3-bar-group")
                        .attr("transform", function(d) { return "translate(0," + y0(d.key) + ")"; });

                // Append text
                group.append("text")
                    .attr("class", "d3-group-label d3-text")
                    .attr("x", -12)
                    .attr("y", function(d) { return y1(d.values[0].value / 2); })
                    .attr("dy", ".35em")
                    .style("text-anchor", "end")
                    .text(function(d) { return d.key; });

                // Add bars
                group.selectAll(".d3-bar")
                    .data(function(d) { return d.values; })
                    .enter()
                    .append("rect")
                        .attr("class", "d3-bar")
                        .attr("x", function(d) { return x(d.date); })
                        .attr("y", function(d) { return y1(d.value); })
                        .attr("width", x.rangeBand())
                        .attr("height", function(d) { return y0.rangeBand() - y1(d.value); })
                        .style("fill", function(d) { return color(d.browser); });


                // Append axes
                // ------------------------------

                // Horizontal
                group.filter(function(d, i) { return !i; }).append("g")
                    .attr("class", "d3-axis d3-axis-horizontal")
                    .attr("transform", "translate(0," + (y0.rangeBand() + 1) + ")")
                    .call(xAxis);

                // Vertical
                var verticalAxis = svg.append("g")
                    .attr("class", "d3-axis d3-axis-vertical")
                    .call(yAxis);

                // Appent text label
                verticalAxis.append("text")
                    .attr('class', 'd3-axis-title browser-label')
                    .attr("x", -12)
                    .attr("y", 12)
                    .attr("dy", ".71em")
                    .style("text-anchor", "end")
                    .text("Browser");


                // Setup layout change
                // ------------------------------

                // Add change event
                d3.selectAll(".stacked-multiple").on("change", change);

                // Change value on page load
                var timeout = setTimeout(function() {
                    d3.selectAll("input[value=\"stacked\"]").property("checked", true).each(change);
                }, 2000);

                // Change function
                function change() {
                    clearTimeout(timeout);
                    if (this.value === "multiples") transitionMultiples();
                    else transitionStacked();
                }

                // Transition to multiples
                function transitionMultiples() {
                    var t = svg.transition().duration(750),
                    g = t.selectAll(".d3-bar-group").attr("transform", function(d) { return "translate(0," + y0(d.key) + ")"; });
                    g.selectAll(".d3-bar").attr("y", function(d) { return y1(d.value); });
                    g.select(".d3-group-label").attr("y", function(d) { return y1(d.values[0].value / 2); })
                }

                // Transition to stacked
                function transitionStacked() {
                    var t = svg.transition().duration(750),
                    g = t.selectAll(".d3-bar-group").attr("transform", "translate(0," + y0(y0.domain()[0]) + ")");
                    g.selectAll(".d3-bar").attr("y", function(d) { return y1(d.value + d.valueOffset) });
                    g.select(".d3-group-label").attr("y", function(d) { return y1(d.values[0].value / 2 + d.values[0].valueOffset); })
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
                x.rangeRoundBands([0, width], .2);

                // Horizontal axis
                svg.selectAll('.d3-axis-horizontal').call(xAxis);


                // Chart elements
                // -------------------------

                // Line path
                svg.selectAll('.d3-bar').attr("x", function(d) { return x(d.date); }).attr("width", x.rangeBand());
            }
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _barStackedMultiple();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    D3BarStackedMultiple.init();
});
