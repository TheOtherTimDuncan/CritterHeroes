'use strict';

var lessPluginAutoPrefix = require('less-plugin-autoprefix');

module.exports = function (gulp, plugins, common) {

    var distCss = common.distPath + '/css';
    var distFonts = common.distPath + '/fonts';
    var srcLess = common.srcPath + '/less';
    var libBootstrap = srcLess + '/bootstrap';
    var stagingCss = srcLess + '/staging';

    var hashes = {};

    var getHashes = function () {

        var collect = function (file, enc, cb) {
            if (file.revHash) {
                hashes[common.path.basename(file.revOrigPath, common.path.extname(file.revOrigPath))] = file.revHash;
            }
            this.push(file);
            return cb();
        };

        return plugins.through2.obj(collect);
    };

    gulp.task('clean-css', function () {
        return plugins.del([distCss + '/**', distFonts + '/**', '!' + common.distPath, libBootstrap, '!' + srcLess, './versioned-css.json']);
    });

    gulp.task('clean-css-staging', ['app-css'], function () {
        return plugins.del(stagingCss);
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

    gulp.task('stage-css', ['clean-css', 'copy-bootstrap', 'copy-bootswatch'], function () {

        var autoprefix = new lessPluginAutoPrefix({ browsers: ['last 2 versions'] });

        return gulp.src(srcLess + '/*.less')
            .pipe(plugins.sourcemaps.init())
            .pipe(plugins.less({ plugins: [autoprefix] }))
            .pipe(plugins.rev())
            .pipe(getHashes())
            .pipe(plugins.sourcemaps.write('../../../' + distCss))
            .pipe(gulp.dest(stagingCss))
            .pipe(plugins.rev.manifest({ path: "versioned-css.json" }))
            .pipe(gulp.dest('./'));

    });

    gulp.task('app-css', ['clean-css', 'stage-css'], function () {

        return gulp.src(stagingCss + '/*.css')
            .pipe(gulp.dest(distCss))
            .pipe(plugins.minifyCss())
            .pipe(plugins.rename({ extname: '.min.css' }))
            .pipe(gulp.dest(distCss));

    });

    return ['clean-css', 'copy-bootstrap', 'copy-bootswatch', 'copy-bootstrap-fonts', 'stage-css', 'app-css', 'clean-css-staging'];

};
