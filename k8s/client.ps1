# Starts an interactive QualificationInterface session in the cluster; the pod is removed on exit.
kubectl run qualification-interface -n qualifications -it --rm --restart=Never `
    --image=qualification-interface:latest --image-pull-policy=Never `
    --env=ASSESSMENT_SERVICE_URL=http://assessment-service:5000
