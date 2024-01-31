import React, { useState } from 'react'
import { LoginResponse, ReadTask, TaskStatus, UpdateTask } from '../model'

interface TaskListItemProps {
    task: ReadTask,
    first: boolean,
    last: boolean,
    user: LoginResponse,
    taskUpdated: (task: ReadTask) => void,
    taskRemoved: (task: ReadTask) => void,
}

const TaskListItem: React.FC<TaskListItemProps> = ({ task, first, last, user, taskUpdated, taskRemoved }) => {
    const [taskSelected, setTaskSelected] = useState(false)

    const onEditFormReset = (e: React.FormEvent<HTMLFormElement>): void => {
        e.preventDefault()

        setTaskSelected((prev) => !prev)
    }

    const onEditFormSubmitted = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
        e.preventDefault()

        const formdata = new FormData(e.currentTarget)

        const title = formdata.get('title')!.toString()
        const description = formdata.get('description')!.toString()
        const taskStatus = parseInt(formdata.get('taskstatus')!.toString())

        const body: UpdateTask = { title, description, taskStatus }

        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/tasks/${task.id}`, {
            method: 'PUT',
            headers: {
                'content-type': 'application/json',
                'authorization': `bearer ${user.jwtToken}`
            },
            body: JSON.stringify(body)
        })

        if (response.ok) {
            const updatedTask = await response.json()

            if (updatedTask) {
                taskUpdated(updatedTask);
            }
        }

        setTaskSelected((prev) => !prev)
    }

    const onDeleteTaskBtnClicked = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.preventDefault()

        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/tasks/${task.id}`, {
            method: 'DELETE',
            headers: {
                'content-type': 'application/json',
                'authorization': `bearer ${user.jwtToken}`
            }
        })

        if (response.ok) {
            taskRemoved(task)
        }

        setTaskSelected((prev) => !prev)
    }

    return (
        <>
            <div onClick={() => setTaskSelected(() => true)} className={`flex items-center shadow px-4 ${task.taskStatus === TaskStatus.INCOMPLETE ? 'bg-red-300' : 'bg-green-300'} ${first ? 'rounded-t' : ''} ${last ? 'rounded-b' : ''} hover:cursor-pointer bg-opacity-80 ${taskSelected ? 'bg-opacity-100' : 'hover:bg-opacity-100'}`}>
                <h6 className='w-5/12'>{task.title}</h6>
                <p className='w-6/12'>{task.description}</p>
                <p className='w-1/12'>{['Incomplete', 'Completed'][task.taskStatus]}</p>
            </div>
            {taskSelected && <div className="absolute top-0 left-0 flex justify-center items-center w-screen h-screen bg-neutral-500 bg-opacity-80" onClick={() => setTaskSelected(() => false)}>
                <form onClick={e => e.stopPropagation()} className='p-10 border rounded-xl shadow w-3/5 mx-auto bg-white' onSubmit={e => onEditFormSubmitted(e)} onReset={e => onEditFormReset(e)}>
                    <div className="flex items-start mb-4">
                        <label htmlFor="title" className='w-1/6 p-2'>Title</label>
                        <input type="text" name="title" id="title" required className='drop-shadow border rounded w-5/6 p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='Title' defaultValue={task.title} />
                    </div>
                    <div className="flex items-start mb-4">
                        <label htmlFor="description" className='w-1/6 p-2'>Description</label>
                        <textarea name="description" id="description" required className='drop-shadow border rounded w-5/6 p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500' placeholder='Description' defaultValue={task.description} />
                    </div>
                    <div className="flex items-start mb-4">
                        <label htmlFor="taskstatus" className='w-1/6 p-2'>Task Status</label>
                        <select name="taskstatus" id="taskstatus" required className='bg-white drop-shadow border rounded w-5/6 p-2 focus:outline-none focus:border-0 focus:border-b-2 focus:border-blue-500'>
                            <option value={0} selected={task.taskStatus === TaskStatus.INCOMPLETE}>Incomplete</option>
                            <option value={1} selected={task.taskStatus === TaskStatus.COMPLETE}>Complete</option>
                        </select>
                    </div>
                    <div className="flex items-center justify-end">
                        <div className="flex items-center me-2 shadow">
                            <button type='submit' className='py-2 px-4 rounded rounded-e-none bg-sky-500'>Save</button>
                            <button className='py-2 px-4 rounded rounded-s-none bg-orange-300'>Cancel</button>
                        </div>
                        <button className='py-2 px-4 rounded shadow bg-red-500' onClick={e => onDeleteTaskBtnClicked(e)}>Delete</button>
                    </div>
                </form>
            </div>}
        </>
    )
}

export { TaskListItem }
