kubectl run qualification-interface -n qualifications -it --rm --restart=Never `
    --image=ryandewet577645/qualification-interface:latest --image-pull-policy=Always `
    --env=ASSESSMENT_SERVICE_URL=http://assessment-service:5000
