/* ------------------------------------------------------------------------------
 *
 *  # D3.js - horizontal bar chart
 *
 *  Demo d3.js horizontal bar chart setup with .csv data source
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var D3BarHorizontal = function() {


    //
    // Setup module components
    //

    // Chart
    var _barHorizontal = function() {
        if (typeof d3 == 'undefined') {
            console.warn('Warning - d3.min.js is not loaded.');
            return;
        }

        // Main variables
        var element = document.getElementById('d3-bar-horizontal'),
            height = 400;


        // Initialize chart only if element exsists in the DOM
        if(element) {

            // Basic setup
            // ------------------------------

            // Define main variables
            var d3Container = d3.select(element),
                margin = {top: 20, right: 10, bottom: 5, left: 40},
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right,
                height = height - margin.top - margin.bottom - 5,
                n = 12;

            // Format data
            var format = d3.format(",.0f");

            // Colors
            var bar_colors = d3.scale.category20c(),
                bar_text_color = '#fff';



            // Construct scales
            // ------------------------------

            // Horizontal
            var x = d3.scale.linear()
                .range([0, width]);

            // Verticals
            var y = d3.scale.ordinal()
                .rangeRoundBands([0, height], .1);



            // Create axes
            // ------------------------------

            // Horizontal
            var xAxis = d3.svg.axis()
                .scale(x)
                .orient("top")
                .tickSize(-height);

            // Vertical
            var yAxis = d3.svg.axis()
                .scale(y)
                .orient("left")
                .tickSize(5);



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

            d3.csv("../../../../demo_data/d3/bars/bars_horizontal.csv", function(data) {

                // Parse numbers, and sort by value.
                data.forEach(function(d) { d.value = +d.value; });
                data.sort(function(a, b) { return b.value - a.value; });


                // Set input domains
                // ------------------------------

                // Horizontal
                x.domain([0, d3.max(data, function(d) { return d.value; })]);

                // Verticals
                y.domain(data.map(function(d) { return d.name; }));


                //
                // Append chart elements
                //

                // Append axes
                // ------------------------------

                // Horizontal
                svg.append("g")
                    .attr("class", "d3-axis d3-axis-horizontal")
                    .call(xAxis);

                // Vertical
                svg.append("g")
                    .attr("class", "d3-axis d3-axis-vertical")
                    .call(yAxis);

                // Remove lines
                svg.selectAll(".d3-axis line, .d3-axis path").attr("stroke-width", 0);


                // Append bars
                // ------------------------------

                // Group bars
                var bar = svg.selectAll(".d3-bar-group")
                    .data(data)
                    .enter()
                    .append("g")
                        .attr("class", "d3-bar-group")
                        .attr("fill", function(d, i) { return bar_colors(i); })
                        .attr("transform", function(d) { return "translate(0," + y(d.name) + ")"; });

                // Add bar
                bar.append("rect")
                    .attr("class", "d3-bar")
                    .attr("width", function(d) { return x(d.value); })
                    .attr("height", y.rangeBand());

                // Add text label
                bar.append("text")
                    .attr("class", "d3-label-value")
                    .attr("x", function(d) { return x(d.value); })
                    .attr("y", y.rangeBand() / 2)
                    .attr("dx", -10)
                    .attr("dy", ".35em")
                    .style("text-anchor", "end")
                    .style("fill", bar_text_color)
                    .style("font-size", 12)
                    .text(function(d) { return format(d.value); });
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
                x.range([0, width]);

                // Horizontal axis
                svg.selectAll('.d3-axis-horizontal').call(xAxis);


                // Chart elements
                // -------------------------

                // Line path
                svg.selectAll('.d3-bar').attr("width", function(d) { return x(d.value); })

                // Text label
                svg.selectAll('.d3-label-value').attr("x", function(d) { return x(d.value); });
            }
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _barHorizontal();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    D3BarHorizontal.init();
});
