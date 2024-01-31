import { useEffect, useState } from "react"
import { CreateTask, LoginResponse, ProfileTypes, ReadTask } from "../model"
import { TaskListItem } from "./task"

interface DashboardProps {
    user: LoginResponse
}

const Dashboard: React.FC<DashboardProps> = ({ user }) => {
    const [tasks, setTasks] = useState<ReadTask[]>([])
    const [showNewTaskForm, setShowNewTaskForm] = useState(false);

    useEffect(() => {
        fetch(`${import.meta.env.VITE_API_URL}/api/tasks`, {
            method: 'GET',
            headers: {
                'content-type': 'application/json',
                'authorization': `bearer ${user.jwtToken}`
            }
        }).then(
            response => response.json(),
            reason => console.log(reason)
        ).then(
            res => setTasks(() => res),
            reason => console.log(reason)
        )
    }, [])

    const updateTask = (task: ReadTask) => {
        setTasks((prev) => {
            const taskIdx = prev.findIndex((t) => t.id === task.id);

            if (taskIdx >= 0) {
                const taskList = [...prev]
                taskList[taskIdx] = task
                return taskList
            }

            return prev
        })
    }

    const removeTask = (task: ReadTask) => {
        setTasks((prev) => prev.filter((t) => t.id !== task.id))
    }

    const onNewTaskFormSubmitted = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        const formdata = new FormData(e.currentTarget)

        const title = formdata.get('title')!.toString()
        const description = formdata.get('description')!.toString()

        const body: CreateTask = { title, description }

        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/tasks`, {
            method: 'POST',
            headers: {
                'content-type': 'application/json',
                'authorization': `bearer ${user.jwtToken}`
            },
            body: JSON.stringify(body)
        })

        if (response.ok) {
            const newTask: ReadTask = await response.json()

            if (newTask) {
                setTasks(prev => [...prev, newTask])
            }
        }

        setShowNewTaskForm(() => false);
    }

    const onNewTaskFormReset = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        setShowNewTaskForm(() => false);
    }

    return (
        <>
            <div className="container mx-auto my-10">
                <div className="flex justify-between items-center">
                    <h1>Tasks</h1>
                    {user.profileType === ProfileTypes.USER && <button className='py-2 px-4 rounded shadow bg-orange-300 hover:shadow-black hover:bg-blue-100' onClick={() => setShowNewTaskForm(() => true)}>Add task</button>}
                </div>
                <div className="rounded bg-neutral-600">
                    <div className='flex items-center px-5 text-white'>
                        <h6 className='w-5/12'>Task Title</h6>
                        <p className='w-6/12'>Description</p>
                        <p className='w-1/12'>Task Status</p>
                    </div>
                    <div className="p-1">
                        {
                            tasks.map(
                                (task, taskIdx) => (
                                    <TaskListItem key={task.id} user={user} task={task} first={taskIdx === 0}
                                        last={taskIdx === tasks.length - 1} taskUpdated={t => updateTask(t)}
                                        taskRemoved={t => removeTask(t)} />
                                )
                            )
                        }
                    </div>
                </div>
            </div>
            {showNewTaskForm && <div className="absolute top-0 left-0 flex justify-center items-center w-screen h-screen bg-neutral-500 bg-opacity-80" onClick={() => setShowNewTaskForm(() => false)}>
                <form onClick={e => e.stopPropagation()} className='p-10 border rounded-xl shadow w-3/5 mx-auto bg-white' onSubmit={e => onNewTaskFormSubmitted(e)} onReset={e => onNewTaskFormReset(e)}>
                    <div className="flex items-start mb-4">
                        <label htmlFor="title" className='w-1/6 p-2'>Title</label>
                        <input type="text" name="title" id="title" required className='drop-shadow border rounded w-5/6 p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='Title' />
                    </div>
                    <div className="flex items-start mb-4">
                        <label htmlFor="description" className='w-1/6 p-2'>Description</label>
                        <textarea name="description" id="description" required className='drop-shadow border rounded w-5/6 p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='Description' />
                    </div>
                    <div className="flex items-center justify-end">
                        <div className="flex items-center me-2 shadow">
                            <button type='submit' className='py-2 px-4 rounded rounded-e-none bg-sky-500'>Save</button>
                            <button type='reset' className='py-2 px-4 rounded rounded-s-none bg-orange-300'>Cancel</button>
                        </div>
                    </div>
                </form>
            </div>}
        </>
    )
}

export { Dashboard }
