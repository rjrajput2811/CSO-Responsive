/* ------------------------------------------------------------------------------
 *
 *  # D3.js - pie chart entry animation
 *
 *  Demo d3.js pie chart setup with .csv data source and loading animation
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var D3PieEntryAnimation = function() {


    //
    // Setup module components
    //

    // Chart
    var _pieEntryAnimation = function() {
        if (typeof d3 == 'undefined') {
            console.warn('Warning - d3.min.js is not loaded.');
            return;
        }

        // Main variables
        var element = document.getElementById('d3-pie-entry-animation'),
            radius = 120;


        // Initialize chart only if element exsists in the DOM
        if(element) {

            // Basic setup
            // ------------------------------

            // Colors
            var pie_colors = d3.scale.category20(),
                pie_text_color = '#fff';


            // Create chart
            // ------------------------------

            // Add SVG element
            var container = d3.select(element).append("svg");

            // Add SVG group
            var svg = container
                .attr("width", radius * 2)
                .attr("height", radius * 2)
                .append("g")
                    .attr("transform", "translate(" + radius + "," + radius + ")");


            // Construct chart layout
            // ------------------------------

            // Arc
            var arc = d3.svg.arc()
                .outerRadius(radius)
                .innerRadius(0);

            // Pie
            var pie = d3.layout.pie()
                .sort(null)
                .value(function(d) { return d.population; });


            // Load data
            // ------------------------------

            d3.csv("../../../../demo_data/d3/pies/pies_basic.csv", function(error, data) {

                // Pull out values
                data.forEach(function(d) {
                    d.population = +d.population;
                });


                //
                // Append chart elements
                //

                // Bind data
                var g = svg.selectAll(".d3-arc")
                    .data(pie(data))
                    .enter()
                    .append("g")
                        .attr("class", "d3-arc");

                // Add arc path
                g.append("path")
                    .attr("d", arc)
                    .attr("class", "d3-slice-border")
                    .style("fill", function(d) { return pie_colors(d.data.age); })
                    .transition()
                        .ease("bounce")
                        .duration(2000)
                        .attrTween("d", tweenPie);

                // Add text labels
                g.append("text")
                    .attr("transform", function(d) { return "translate(" + arc.centroid(d) + ")"; })
                    .attr("dy", ".35em")
                    .style("opacity", 0)
                    .style("fill", pie_text_color)
                    .style("font-size", 12)
                    .style("text-anchor", "middle")
                    .text(function(d) { return d.data.age; })
                    .transition()
                        .ease("linear")
                        .delay(2000)
                        .duration(500)
                        .style("opacity", 1)


                // Tween
                function tweenPie(b) {
                    b.innerRadius = 0;
                    var i = d3.interpolate({startAngle: 0, endAngle: 0}, b);
                        return function(t) { return arc(i(t));
                    };
                }


                // Animate pie
                // ------------------------------

                d3.select('.pie-animation').on('click', function () {

                    // Remove old paths
                    svg.selectAll("path, text").remove();

                    // Arc path
                    g.append("path")
                        .attr("d", arc)
                        .attr("class", "d3-slice-border")
                        .style("fill", function(d) { return pie_colors(d.data.age); })
                        .transition()
                            .ease("bounce")
                            .duration(2000)
                            .attrTween("d", tweenPie);

                    // Text labels
                    g.append("text")
                        .attr("transform", function(d) { return "translate(" + arc.centroid(d) + ")"; })
                        .attr("dy", ".35em")
                        .style("opacity", 0)
                        .style("fill", pie_text_color)
                        .style("font-size", 12)
                        .style("text-anchor", "middle")
                        .text(function(d) { return d.data.age; })
                        .transition()
                            .ease("linear")
                            .delay(2000)
                            .duration(500)
                            .style("opacity", 1)
                });
            });
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _pieEntryAnimation();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    D3PieEntryAnimation.init();
});
