﻿'use strict';

var lessPluginAutoPrefix = require('less-plugin-autoprefix');

module.exports = function (gulp, plugins, common) {

    var distCss = common.distPath + '/css';
    var distFonts = common.distPath + '/fonts';
    var distImages = common.distPath + '/images';

    var srcLess = common.srcPath + '/css';
    var srcImages = common.srcPath + '/images';

    var libBootstrap = srcLess + '/bootstrap';

    gulp.task('clean-css', function () {
        return plugins.del([distCss + '/**/*', distFonts + '/**/*', distImages + '/**/*', libBootstrap + '/**/*', './versioned-css.json'], { debug: true });
    });

    gulp.task('copy-images', ['clean-css'], function () {

        return gulp.src(srcImages + '/**/*.*')
            .pipe(gulp.dest(distImages));

    });

    gulp.task('copy-bootstrap', ['clean-css'], function () {

        return gulp.src(common.bowerBase + '/bootstrap/less/**/*.less')
            .pipe(gulp.dest(libBootstrap));

    });

    gulp.task('copy-bootswatch', ['clean-css', 'copy-bootstrap'], function () {

        return gulp.src(common.bowerBase + '/bootswatch/yeti/*.less')
            .pipe(gulp.dest(libBootstrap));

    });

    gulp.task('copy-bootstrap-fonts', ['clean-css'], function () {

        return gulp.src(common.bowerBase + '/bootstrap/dist/fonts/**/')
            .pipe(gulp.dest(distFonts));

    });

    gulp.task('app-css', ['clean-css', 'copy-bootstrap', 'copy-bootswatch'], function () {

        var autoprefix = new lessPluginAutoPrefix({ browsers: ['last 2 versions'] });

        return gulp.src(srcLess + '/*.less', { base: common.srcPath })
            .pipe(plugins.less({ plugins: [autoprefix] }))
            .pipe(gulp.dest(common.distPath))
            .pipe(plugins.sourcemaps.init())
            .pipe(plugins.minifyCss())
            .pipe(plugins.rename({ extname: '.min.css' }))
            .pipe(plugins.rev())
            .pipe(plugins.sourcemaps.write('.'))
            .pipe(gulp.dest(common.distPath))
            .pipe(plugins.rev.manifest("versioned-css.json"))
            .pipe(gulp.dest('./'));

    });

    return ['clean-css', 'copy-images', 'copy-bootstrap', 'copy-bootswatch', 'copy-bootstrap-fonts', 'app-css'];

};
